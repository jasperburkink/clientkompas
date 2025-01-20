using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;

namespace Application.Users.Commands.CreateUser
{
    [Authorize(Policy = Policies.UserManagement)]
    public record CreateUserCommand : IRequest<CreateUserCommandDto>
    {
        public string? FirstName { get; set; }

        public string? PrefixLastName { get; set; }

        public string? LastName { get; set; }

        public string? EmailAddress { get; set; }

        public string? TelephoneNumber { get; set; }
    }

    public class CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IIdentityService identityService,
        ITokenService tokenService, IPasswordService passwordService, IEmailService emailService)
        : IRequestHandler<CreateUserCommand, CreateUserCommandDto>
    {
        private const int DEFAULT_PASSWORD_LENGTH = 8;

        public async Task<CreateUserCommandDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                PrefixLastName = request.PrefixLastName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                TelephoneNumber = request.TelephoneNumber,
                IsDeactivated = false,
                CreatedByUserId = 0
            };

            if (await unitOfWork.UserRepository.AnyAsync(u => u.EmailAddress.ToLower() == request.EmailAddress.ToLower(), cancellationToken))
            {
                // TODO: custom exception
                throw new Exception("Dit emailadres is al ingebruik.");
            }

            // Add CVS user
            await unitOfWork.UserRepository.InsertAsync(user);
            await unitOfWork.SaveAsync(cancellationToken);

            var password = passwordService.GeneratePassword(DEFAULT_PASSWORD_LENGTH);

            // Create Authentication user
            var (result, userId) = await identityService.CreateUserAsync(request.EmailAddress, password, user.Id);
            if (!result.Succeeded)
            {
                await RemoveUser(user, cancellationToken);

                // TODO: Custom exception
                throw new Exception();
            }

            var authenticationUser = await identityService.GetUserAsync(userId);

            // Create link with token for changing temporary password
            var token = await tokenService.GenerateTokenAsync(authenticationUser, "TemporaryPasswordToken"); // TODO: name in constants
            var link = new Uri($"https://localhost:3000/ChangePassword/{token}");

            // Send email with
            emailService.SendEmailAsync(request.EmailAddress, "Gebruikersgegevens",
                $"""
                Beste {request.FirstName},

                Hierbij uw tijdelijke logingegevens.
                Gebruikersnaam: {request.EmailAddress}
                Wachtwoord:     {password}

                Bovenstaande wachtwoord is tijdelijk en dient zo spoedig mogelijk te worden aangepast.
                Via de volgende link kunt u uw tijdelijke wachtwoord aanpassen: {link}
                """); // TODO: Change to new emailmodule

            return mapper.Map<CreateUserCommandDto>(user);
        }

        private async Task RemoveUser(User user, CancellationToken cancellationToken)
        {
            await unitOfWork.UserRepository.DeleteAsync(user);
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
