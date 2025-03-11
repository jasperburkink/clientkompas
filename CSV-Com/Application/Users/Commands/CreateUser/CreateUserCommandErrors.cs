using Application.Common.Models;

namespace Application.Users.Commands.CreateUser
{
    public static class CreateUserCommandErrors
    {
        public static readonly Error EmailAddressInUse = new($"{nameof(CreateUserCommandErrors)}.{nameof(EmailAddressInUse)}", "Emailaddress '{0}' is already in use.");

        public static readonly Error TelephoneNumberInUse = new($"{nameof(CreateUserCommandErrors)}.{nameof(TelephoneNumberInUse)}", "Telephonenumber '{0}' is already in use.");

        public static readonly Error CreatingAuthenticationUser = new($"{nameof(CreateUserCommandErrors)}.{nameof(CreatingAuthenticationUser)}", "Something went wrong while creating an authentication user with email '{0}'. ErrorMessage:'{1}'.");

        public static readonly Error AddingRoleToAuthenticationUser = new($"{nameof(CreateUserCommandErrors)}.{nameof(AddingRoleToAuthenticationUser)}", "Something went wrong while adding a role '{0}' to an user '{1}'.ErrorMessage:'{2}'.");

        public static readonly Error NoUserLoggedIn = new($"{nameof(CreateUserCommandErrors)}.{nameof(NoUserLoggedIn)}", "There's no user logged in right now!");
    }
}
