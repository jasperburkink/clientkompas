using System.Net;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.CVS;
using Application.Common.Models;
using Application.Common.Security;
using Ardalis.GuardClauses;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using Domain.CVS.Domain;

namespace Application.Users.Commands.SendTemporaryPasswordLinkCommand
{
    [Authorize(Policy = Policies.UserManagement)]
    public record SendTemporaryPasswordLinkCommand : IRequest<Result<SendTemporaryPasswordLinkCommandDto>>
    {
        public string? UserId { get; set; }
    }

    public class SendTemporaryPasswordLinkCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IIdentityService identityService,
        ITokenService tokenService, IPasswordService passwordService, IEmailService emailService)
        : IRequestHandler<SendTemporaryPasswordLinkCommand, Result<SendTemporaryPasswordLinkCommandDto>>
    {
        private const int MAX_TIMES_SEND_TEMP_PASSWORD_TOKEN = 1;

        public async Task<Result<SendTemporaryPasswordLinkCommandDto>> Handle(SendTemporaryPasswordLinkCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.NullOrEmpty(request.UserId);

            var authenticationUser = await identityService.GetUserAsync(request.UserId);

            if (!authenticationUser.HasTemporaryPassword)
            {
                return Result<SendTemporaryPasswordLinkCommandDto>.Failure("This user has not got a temporary password.");
            }

            var currentToken = (await tokenService.GetValidTokensByUserAsync(authenticationUser.Id, "TemporaryPasswordToken")).FirstOrDefault(); // TODO: name in constants

            if (currentToken == null)
            {
                return Result<SendTemporaryPasswordLinkCommandDto>.Failure("Temporary password token not found for user.");
            }

            var cvsUser = unitOfWork.UserRepository.GetByID(authenticationUser.CVSUserId);

            if (cvsUser == null)
            {
                return Result<SendTemporaryPasswordLinkCommandDto>.Failure("User not found.");
            }

            // When token has been sent more than n times, send temporary password link with token to user.
            if (authenticationUser.TemporaryPasswordTokenCount <= MAX_TIMES_SEND_TEMP_PASSWORD_TOKEN)
            {
                var result = await HandleResendTemporaryPasswordLink(identityService, tokenService, emailService, authenticationUser, cvsUser);

                if (!result.Succeeded)
                {
                    return Result<SendTemporaryPasswordLinkCommandDto>.Failure(result.Errors);
                }
            }
            // Otherwise send contact info from user that created this user.
            else
            {
                var result = await HandleUserContactInformation(emailService, cvsUser);

                if (!result.Succeeded)
                {
                    return Result<SendTemporaryPasswordLinkCommandDto>.Failure(result.Errors);
                }
            }

            return Result.Success(new SendTemporaryPasswordLinkCommandDto
            {
                UserId = cvsUser.Id
            });
        }

        private static async Task<Result> HandleResendTemporaryPasswordLink(IIdentityService identityService, ITokenService tokenService, IEmailService emailService, AuthenticationUser authenticationUser, User cvsUser)
        {
            var newToken = await tokenService.GenerateTokenAsync(authenticationUser, "TemporaryPasswordToken"); // TODO: name in constants
            var link = new Uri($"https://localhost:3000/ChangePassword/{WebUtility.UrlEncode(newToken)}");

            var emailAddress = authenticationUser.UserName ?? authenticationUser.Email;

            if (string.IsNullOrEmpty(emailAddress))
            {
                return Result.Failure("No emailaddress found for this user.");
            }

            await emailService.SendEmailAsync(emailAddress, "Gebruikersgegevens",
                $"""
                    Beste {cvsUser.FirstName},

                    Het eerder verstuurde wachtwoord is tijdelijk en dient zo spoedig mogelijk te worden aangepast.
                    Via de volgende link kunt u uw tijdelijke wachtwoord aanpassen: {link}
                    """); // TODO: Change to new emailmodule

            authenticationUser.TemporaryPasswordTokenCount++;

            await identityService.UpdateUserAsync(authenticationUser);

            return Result.Success();
        }

        private static async Task<Result> HandleUserContactInformation(IEmailService emailService, User cvsUser)
        {
            if (cvsUser == null)
            {
                return Result.Failure("User not found.");
            }

            if (cvsUser.CreatedByUser == null)
            {
                return Result.Failure("User which create this user not found.");
            }

            var emailAddress = cvsUser.CreatedByUser.EmailAddress;

            if (string.IsNullOrEmpty(emailAddress))
            {
                return Result.Failure("No emailaddress found for this user.");
            }

            await emailService.SendEmailAsync(emailAddress, "Gebruikersgegevens",
                $"""
                    Beste {cvsUser.FirstName},

                    Uw link om uw tijdelijke wachtwoord te kunnen resetten is helaas verlopen.
                    Mocht u een nieuwe link willen ontvangen, dan verzoeken wij u om contact op te nemen met {cvsUser.CreatedByUser.FirstName} {cvsUser.PrefixLastName + " " ?? ""}{cvsUser.LastName}.
                    Dat kan via het volgende e-mailadres: {emailAddress}
                    """); // TODO: Change to new emailmodule

            return Result.Success();
        }
    }
}
