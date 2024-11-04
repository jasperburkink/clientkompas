using Application.Authentication.Commands.ResetPassword;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using TestData;
using TestData.Authentication;

namespace Application.FunctionalTests.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandTests : BaseTestFixture
    {
        private AuthenticationUser _authenticationUser;
        private ResetPasswordCommand _command;

        [SetUp]
        public async Task SetUp()
        {
            ITestDataGenerator<AuthenticationUser> testDataGeneratorAuthenticationUser = new AuthenticationUserDataGenerator();
            _authenticationUser = testDataGeneratorAuthenticationUser.Create();

            //await AddAsync<AuthenticationUser, AuthenticationDbContext>(_authenticationUser);
            var initialPassword = FakerConfiguration.Faker.Internet.Password(AuthenticationUserConstants.PASSWORD_MINLENGTH) + "!";
            await CreateUserAsync(_authenticationUser.Email!, initialPassword);

            var password = FakerConfiguration.Faker.Internet.Password(AuthenticationUserConstants.PASSWORD_MINLENGTH) + "!";

            var token =

            _command = new ResetPasswordCommand
            {
                EmailAddress = _authenticationUser.Email!,
                NewPassword = password,
                NewPasswordRepeat = password,
                Token = FakerConfiguration.Faker.Random.String2(20)
            };
        }

        [Test]
        public async Task Handle_CorrectFlow_SuccessIsTrue()
        {
            // Arrange

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }


        //Handle_CorrectFlow_SuccessIsTrue

        //    Handle_EmailAddressIsNull_SuccessIsFalse

        //    Handle_TokenIsNull_SuccessIsFalse

        //    Handle_NewPasswordIsNull_SuccessIsFalse

        //    Handle_ResetPasswordAsyncThrowsException_SuccessIsFalse
    }


}
