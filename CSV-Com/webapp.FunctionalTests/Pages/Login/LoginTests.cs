using Application.Authentication.Commands.Login;

namespace WebApp.FunctionalTests.Pages.Login
{
    [TestFixture]
    public class LoginTests : PageTest
    {
        private const string Url = Constants.Url + "Login/";

        private readonly string
            _username = "SystemOwner", //FakerConfiguration.Faker.Person.Email,
            _password = "SystemOwner1!"; //FakerConfiguration.Faker.Internet.Password();

        // TODO: logging in can only happen after an user is created.
        //[Skip]
        //[Ignore("Playwright does not work in the pipeline")]
        [Test, Order(1)]
        public async Task Login_FillInUserNameAndPasswordAndClickLogin_UserShouldBeLoggedIn()
        {
            // Arrange

            // Act
            await Page.GotoAsync(Url);

            await Page.GetByTestId(nameof(LoginCommand.UserName).ToLower()).FillAsync(_username);

            await Page.GetByTestId(nameof(LoginCommand.Password).ToLower()).FillAsync(_password);

            await Page.GetByTestId("button.login").ClickAsync();

            // Assert
            await Expect(Page.GetByTestId("message.confirm")).ToContainTextAsync("Inloggen succesvol");
        }
    }
}
