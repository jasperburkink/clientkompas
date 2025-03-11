using System.Net;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.CVS;
using Application.Common.Models;
using Application.Common.Security;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using Domain.CVS.Domain;

namespace Application.Users.Commands.CreateUser
{
    [Authorize(Policy = Policies.UserManagement)]
    public record CreateUserCommand : IRequest<Result<CreateUserCommandDto>>
    {
        public string? FirstName { get; set; }

        public string? PrefixLastName { get; set; }

        public string? LastName { get; set; }

        public string? EmailAddress { get; set; }

        public string? TelephoneNumber { get; set; }

        public string? RoleName { get; set; }
    }

    public class CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IIdentityService identityService,
        ITokenService tokenService, IPasswordService passwordService, IEmailService emailService)
        : IRequestHandler<CreateUserCommand, Result<CreateUserCommandDto>>
    {
        private const int DEFAULT_PASSWORD_LENGTH = 8;

        public async Task<Result<CreateUserCommandDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var currentLoggedInUserResult = await GetCurrentLoggedInUserId(identityService);
            if (!currentLoggedInUserResult.Succeeded)
            {
                return Result<CreateUserCommandDto>.Failure(currentLoggedInUserResult.Errors);
            }

            var cvsUserResult = await CreateCVSUser(unitOfWork, request, currentLoggedInUserResult.Value, cancellationToken);
            if (!cvsUserResult.Succeeded)
            {
                return Result<CreateUserCommandDto>.Failure(cvsUserResult.Errors);
            }

            var password = passwordService.GeneratePassword(DEFAULT_PASSWORD_LENGTH);

            var authenticationUserResult = await CreateAuthenticationUser(identityService, request, cvsUserResult.Value, password, cancellationToken);
            if (!authenticationUserResult.Succeeded)
            {
                return Result<CreateUserCommandDto>.Failure(authenticationUserResult.Errors);
            }

            var authenticationUser = await identityService.GetUserAsync(authenticationUserResult.Value);

            var link = await CreateChangePasswordLink(tokenService, authenticationUser);

            await SendEmail(emailService, request, password, link);

            return Result<CreateUserCommandDto>.Success(mapper.Map<CreateUserCommandDto>(cvsUserResult.Value));
        }

        private static async Task<Result<User>> CreateCVSUser(IUnitOfWork unitOfWork, CreateUserCommand request, int? currentLoggedInUserId, CancellationToken cancellationToken)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                PrefixLastName = request.PrefixLastName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                TelephoneNumber = request.TelephoneNumber,
                CreatedByUserId = currentLoggedInUserId
            };

            if (await unitOfWork.UserRepository.AnyAsync(u => u.EmailAddress.ToUpper() == request.EmailAddress.ToUpper(), cancellationToken))
            {
                return Result<User>.Failure(CreateUserCommandErrors.EmailAddressInUse.WithParams(request.EmailAddress));
            }

            if (await unitOfWork.UserRepository.AnyAsync(u => u.TelephoneNumber.ToUpper() == request.TelephoneNumber.ToUpper(), cancellationToken))
            {
                return Result<User>.Failure(CreateUserCommandErrors.TelephoneNumberInUse.WithParams(request.TelephoneNumber));
            }

            await unitOfWork.UserRepository.InsertAsync(user);
            await unitOfWork.SaveAsync(cancellationToken);

            return Result<User>.Success(user);
        }

        private async Task<Result<string>> CreateAuthenticationUser(IIdentityService identityService, CreateUserCommand request, User user, string password, CancellationToken cancellationToken)
        {
            var (result, userId) = await identityService.CreateUserAsync(request.EmailAddress, password, user.Id);

            if (!result.Succeeded)
            {
                await RemoveUser(user, cancellationToken);
                return Result<string>.Failure(CreateUserCommandErrors.CreatingAuthenticationUser.WithParams(request.EmailAddress, result.ErrorMessage));
            }

            var addToRoleResult = await identityService.AddUserToRoleAsync(userId, request.RoleName);

            if (!addToRoleResult.Succeeded)
            {
                await identityService.RemoveUserAsync(userId);
                await RemoveUser(user, cancellationToken);
                return Result<string>.Failure(CreateUserCommandErrors.AddingRoleToAuthenticationUser.WithParams(request.RoleName, request.EmailAddress, addToRoleResult.ErrorMessage));
            }

            return Result<string>.Success(userId);
        }

        private static async Task<Result<int?>> GetCurrentLoggedInUserId(IIdentityService identityService)
        {
            var currentLoggedInUserId = await identityService.GetCurrentLoggedInUserId();

            if (currentLoggedInUserId == 0)
            {
                return Result<int?>.Failure(CreateUserCommandErrors.NoUserLoggedIn);
            }

            return Result<int?>.Success(currentLoggedInUserId);
        }

        private static async Task<Uri> CreateChangePasswordLink(ITokenService tokenService, IAuthenticationUser authenticationUser)
        {
            var token = await tokenService.GenerateTokenAsync(authenticationUser, "TemporaryPasswordToken"); // TODO: name in constants
            return new Uri($"https://localhost:3000/ChangePassword/{WebUtility.UrlEncode(token)}");
        }

        private static async Task SendEmail(IEmailService emailService, CreateUserCommand request, string password, Uri link)
        {
            await emailService.SendEmailAsync(request.EmailAddress, "Gebruikersgegevens",
                            $"""
                            Beste {request.FirstName},

                            Hierbij uw tijdelijke logingegevens.
                            Gebruikersnaam: {request.EmailAddress}
                            Wachtwoord:     {password}

                            Bovenstaande wachtwoord is tijdelijk en dient zo spoedig mogelijk te worden aangepast.
                            Via de volgende link kunt u uw tijdelijke wachtwoord aanpassen: {link}
                            """); // TODO: Change to new emailmodule
        }

        private async Task RemoveUser(User user, CancellationToken cancellationToken)
        {
            await unitOfWork.UserRepository.DeleteAsync(user);
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
