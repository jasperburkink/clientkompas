using Microsoft.Playwright;

namespace WebApp.FunctionalTests.Pages
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        private const string Url = Constants.Url + "Clients";

        [Test]
        [Skip]
        [Ignore("Playwright does not work in the pipeline")]
        public async Task PageLoaded_Title_ShouldBeCVS()
        {
            // Arrange
            var title = "CVS";

            // Act
            await Page.GotoAsync(Url);

            // Assert
            await Expect(Page).ToHaveTitleAsync(new Regex(title));
        }

        [Test]
        [Skip]
        [Ignore("Playwright does not work in the pipeline")]
        public async Task SearchClients_SearchForNamesWithValueJan_ShouldReturnMultipleClients()
        {
            // Arrange
            var searchTerm = "Jan";
            var resultName = "Jansen, Jan";

            // Act
            await Page.GotoAsync(Url);
            await Page.Locator("#sidebarArrow").ClickAsync();
            await Page.GetByPlaceholder("Zoeken").ClickAsync();
            await Page.GetByPlaceholder("Zoeken").FillAsync(searchTerm);

            // Assert
            await Expect(Page.GetByRole(AriaRole.List)).ToContainTextAsync(resultName);
        }

        [Test]
        [Skip]
        [Ignore("Playwright does not work in the pipeline")]
        public async Task SearchClients_SearchForNamesWithNonExistingValue_ShouldReturnNoClientsFound()
        {
            // Arrange
            var searchTerm = "Search for client that does not exists";
            var resultText = "Er zijn geen cliënten gevonden die aan de zoekcriteria voldoen.";

            // Act
            await Page.GotoAsync(Url);
            await Page.Locator("#sidebarArrow").ClickAsync();
            await Page.GetByPlaceholder("Zoeken").ClickAsync();
            await Page.GetByPlaceholder("Zoeken").FillAsync(searchTerm);

            // Assert
            await Expect(Page.Locator("#sidebarGray")).ToContainTextAsync(resultText);
        }
    }
}
