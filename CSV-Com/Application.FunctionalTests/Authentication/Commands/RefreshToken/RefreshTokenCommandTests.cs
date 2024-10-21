using Application.Authentication.Commands.RefreshToken;
using TestData;

namespace Application.FunctionalTests.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandTests
    {
        private string _password;
        private RefreshTokenCommand _command;

        [SetUp]
        public async Task SetUp()
        {
            _password = PasswordGenerator.GenerateSecurePassword(16);

            _command = new RefreshTokenCommand
            {
                RefreshToken = "Test"
            };
        }
    }
}
