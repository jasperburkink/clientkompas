using Microsoft.Playwright;

namespace WebApp.FunctionalTests.Pages
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ClientEditorTests : PageTest
    {
        private const string Url = Constants.Url + "Clients/Edit/";

        //[Test]
        [Skip]
        [Ignore("Playwright does not work in the pipeline")]
        public async Task DeactivateClient_Client_ShouldBeShownInSearchResults()
        {
            // Arrange
            var title = "CVS";

            // Act
            await Page.GotoAsync(Url);
            await Page.Locator("#sidebarArrow").ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Jansen, Jan" }).ClickAsync();
            await Page.GetByTestId("button_test").ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Bevestigen" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Ok" }).ClickAsync();
            await Page.Locator("#sidebarArrow").ClickAsync();
            await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Jansen, Jan" })).ToBeVisibleAsync();

            // Assert
            await Expect(Page).ToHaveTitleAsync(new Regex(title));
        }
    }
}
