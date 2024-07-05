using Microsoft.Playwright;
using TestData;

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
            var name = "Jansen, Jan";

            // Act
            await Page.GotoAsync(Url);
            await Page.Locator("#sidebarArrow").ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Jansen, Jan" }).ClickAsync();
            await Page.GetByTestId("button_test").ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Bevestigen" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Ok" }).ClickAsync();
            await Page.Locator("#sidebarArrow").ClickAsync();

            // Assert
            await Expect(Page.GetByRole(AriaRole.Link, new() { Name = name })).ToBeVisibleAsync();
        }

        //[Test]
        [Skip]
        [Ignore("Playwright does not work in the pipeline")]
        public async Task CreateClient_FillInClientDataAndSave_ClientShouldBeAdded()
        {
            // Arrange
            var emailAddress = FakerConfiguration.Faker.Person.Email;

            // TODO: change to datatestid selectors?
            // Act
            await Page.GotoAsync(Url);
            await Page.GetByPlaceholder("Voornaam").ClickAsync();
            await Page.GetByPlaceholder("Voornaam").FillAsync("John");
            await Page.GetByPlaceholder("Voornaam").PressAsync("Tab");
            await Page.GetByPlaceholder("b.v. A B").FillAsync("J");
            await Page.GetByPlaceholder("b.v. A B").PressAsync("Tab");
            await Page.GetByPlaceholder("b.v. de").PressAsync("Tab");
            await Page.GetByPlaceholder("Achternaam").FillAsync("Doe");
            await Page.GetByPlaceholder("Achternaam").PressAsync("Tab");
            await Page.GetByPlaceholder("Adres").FillAsync("Dorpstraat");
            await Page.GetByPlaceholder("Adres").PressAsync("Tab");
            await Page.GetByPlaceholder("b.v. 11").FillAsync("4");
            await Page.GetByPlaceholder("b.v. 11").PressAsync("Tab");
            await Page.GetByPlaceholder("b.v. A", new() { Exact = true }).FillAsync("a");
            await Page.GetByPlaceholder("b.v. A", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByPlaceholder("b.v. 1234 AA").FillAsync("1234 AB");
            await Page.GetByPlaceholder("b.v. 1234 AA").PressAsync("Tab");
            await Page.GetByPlaceholder("Woonplaats").FillAsync("Utrecht");
            await Page.GetByPlaceholder("Woonplaats").PressAsync("Tab");
            await Page.GetByPlaceholder("b.v. 0543-").FillAsync("0123456789");
            await Page.GetByPlaceholder("b.v. 0543-").PressAsync("Tab");
            await Page.GetByPlaceholder("b.v. mail@mailbox.com").FillAsync(emailAddress);
            await Page.GetByLabel("Choose date").First.ClickAsync();
            await Page.GetByLabel("calendar view is open, switch").ClickAsync();
            await Page.GetByRole(AriaRole.Radio, new() { Name = "1989" }).ClickAsync();
            await Page.GetByRole(AriaRole.Gridcell, new() { Name = "19" }).ClickAsync();
            await Page.GetByPlaceholder("Naam", new() { Exact = true }).ClickAsync();
            await Page.GetByPlaceholder("Naam", new() { Exact = true }).FillAsync("Jane Doe");
            await Page.GetByPlaceholder("Naam", new() { Exact = true }).PressAsync("Tab");
            await Page.GetByPlaceholder("Telefoonnr.").FillAsync("0987654321");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Voeg nog een contactpersoon" }).ClickAsync();
            await Page.GetByPlaceholder("Naam").Nth(3).ClickAsync();
            await Page.GetByPlaceholder("Naam").Nth(3).FillAsync("Willem Doe");
            await Page.GetByPlaceholder("Naam").Nth(3).PressAsync("Tab");
            await Page.GetByPlaceholder("Telefoonnr.").Nth(1).FillAsync("0654789123");
            await Page.GetByPlaceholder("Voeg opmerkingen toe").ClickAsync();
            await Page.GetByPlaceholder("Voeg opmerkingen toe").FillAsync("Wie is John Doe?");
            await Page.Locator("select[name=\"diagnoses\"]").SelectOptionAsync(new[] { "1" });
            await Page.Locator("select[name=\"benefitforms\"]").SelectOptionAsync(new[] { "1" });
            await Page.Locator("div").Filter(new() { HasTextRegex = new Regex("^UitkeringsvormKies uit de lijstBijstandWIA$") }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Diagnose\\(s\\)Kies uit de lijstDyslexiaDepression$") }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Kies uit de lijstWIA$") }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.Locator("select[name=\"maritalstatus\"]").SelectOptionAsync(new[] { "1" });
            await Page.Locator("select[name=\"driverslicences\"]").SelectOptionAsync(new[] { "1" });
            await Page.Locator("div").Filter(new() { HasTextRegex = new Regex("^RijbewijsKies uit de lijstB \\(Auto\\)A \\(Motor\\)$") }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.Locator("select[name=\"driverslicences\"]").SelectOptionAsync(new[] { "2" });
            await Page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Kies uit de lijstA \\(Motor\\)$") }).GetByRole(AriaRole.Button).ClickAsync();
            await Page.Locator("select[name=\"Werkt bij\"]").SelectOptionAsync(new[] { "5" });
            await Page.GetByPlaceholder("Functie").ClickAsync();
            await Page.GetByPlaceholder("Functie").FillAsync("Directeur");
            await Page.Locator("select[name=\"Contract\"]").SelectOptionAsync(new[] { "1" });
            await Page.GetByLabel("Choose date").Nth(1).ClickAsync();
            await Page.Locator("div").Filter(new() { HasTextRegex = new Regex("^juli 2024$") }).Nth(1).ClickAsync();
            await Page.GetByRole(AriaRole.Radio, new() { Name = "1997" }).ClickAsync();
            await Page.GetByRole(AriaRole.Gridcell, new() { Name = "7", Exact = true }).ClickAsync();
            await Page.GetByLabel("Choose date", new() { Exact = true }).ClickAsync();
            await Page.Locator("div").Filter(new() { HasTextRegex = new Regex("^juli 2024$") }).Nth(1).ClickAsync();
            await Page.GetByRole(AriaRole.Radio, new() { Name = "2006" }).ClickAsync();
            await Page.GetByTestId("button.save").ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Bevestigen" }).ClickAsync();

            // Assert
            await Expect(Page.GetByText("John Doe", new() { Exact = true })).ToBeVisibleAsync();
            await Expect(Page.GetByText("Jane Doe")).ToBeVisibleAsync();
            await Expect(Page.GetByText("Dyslexia")).ToBeVisibleAsync();
            await Expect(Page.GetByText("Unmarried")).ToBeVisibleAsync();
            await Expect(Page.GetByText("B, A")).ToBeVisibleAsync();
            await Expect(Page.GetByText("Bijstand")).ToBeVisibleAsync();
            await Expect(Page.GetByText(emailAddress)).ToBeVisibleAsync();
        }
    }
}
