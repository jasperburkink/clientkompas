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

        public int? LicenceId { get; set; } // TODO: Is not yet implemented

        public string? RoleName { get; set; }
    }

    public class CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IIdentityService identityService,
        ITokenService tokenService, IPasswordService passwordService, IEmailService emailService)
        : IRequestHandler<CreateUserCommand, Result<CreateUserCommandDto>>
    {
        private const int DEFAULT_PASSWORD_LENGTH = 8;

        public async Task<Result<CreateUserCommandDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var currentLoggedInUserId = await GetCurrentLoggedInUserId(identityService);

            var cvsUser = await CreateCVSUser(unitOfWork, request, currentLoggedInUserId, cancellationToken);

            var password = passwordService.GeneratePassword(DEFAULT_PASSWORD_LENGTH);

            var authenticationUserId = await CreateAuthenticationUser(identityService, request, cvsUser, password, cancellationToken);

            var authenticationUser = await identityService.GetUserAsync(authenticationUserId);

            var link = await CreateChangePasswordLink(tokenService, authenticationUser);

            await SendEmail(emailService, request, password, link);

            return Result.Success(mapper.Map<CreateUserCommandDto>(cvsUser));
        }

        private async Task<string> CreateAuthenticationUser(IIdentityService identityService, CreateUserCommand request, User user, string password, CancellationToken cancellationToken)
        {
            // Create Authentication user
            var (result, userId) = await identityService.CreateUserAsync(request.EmailAddress, password, user.Id);

            if (!result.Succeeded)
            {
                await RemoveUser(user, cancellationToken);
                Result.Failure($"Something went wrong while createing an authentication user with email '{request.EmailAddress}'.");
            }

            await identityService.AddUserToRoleAsync(userId, request.RoleName);
            return userId;
        }

        private static async Task<User> CreateCVSUser(IUnitOfWork unitOfWork, CreateUserCommand request, int? currentLoggedInUserId, CancellationToken cancellationToken)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                PrefixLastName = request.PrefixLastName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                TelephoneNumber = request.TelephoneNumber,
                IsDeactivated = false,
                CreatedByUserId = currentLoggedInUserId
            };

            if (await unitOfWork.UserRepository.AnyAsync(u => u.EmailAddress.ToLower() == request.EmailAddress.ToLower(), cancellationToken))
            {
                Result.Failure($"Emailaddress '{request.EmailAddress}' is already in use.");
            }

            await unitOfWork.UserRepository.InsertAsync(user);
            await unitOfWork.SaveAsync(cancellationToken);
            return user;
        }

        private static async Task<int?> GetCurrentLoggedInUserId(IIdentityService identityService)
        {
            var currentLoggedInUserId = await identityService.GetCurrentLoggedInUserId();

            if (currentLoggedInUserId == 0)
            {
                Result.Failure("There's no user logged in right now!");
            }

            return currentLoggedInUserId;
        }

        private static async Task<Uri> CreateChangePasswordLink(ITokenService tokenService, AuthenticationUser authenticationUser)
        {
            var token = await tokenService.GenerateTokenAsync(authenticationUser, "TemporaryPasswordToken"); // TODO: name in constants
            return new Uri($"https://localhost:3000/ChangePassword/{token}");
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
