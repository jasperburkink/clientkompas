using Domain.CVS.Domain;
using Domain.CVS.Enums;
using TestData;

namespace WebApp.FunctionalTests.Pages
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ClientEditorTests : PageTest
    {
        // TODO: test mobile

        private const string Url = Constants.Url + "Clients/";
        private int _clientId;

        [Test, Order(1)]
        public async Task CreateClient_FillInClientDataAndSave_ClientShouldBeAdded()
        {
            // Arrange
            string firstName = "John", initials = "J", lastname = "Doe", streetName = "Dorpstraat", housenumber = "4",
            houseNumberAddition = "a", postalCode = "1234 AB", residence = "Utrecht", telephoneNumber = "0123456789",
            emailAddress = FakerConfiguration.Faker.Person.Email, dateOfBirth = "02-08-1989", emergencyPerson1Name = "Jane Doe",
            emergencyPerson1TelephoneNumber = "0987654321", emergencyPerson2Name = "Willem Doe", emergencyPerson2TelephoneNumber = "0654789123",
            remark = "Wie is John Doe?", diagnosis1 = "Depression", diagnosis2 = "Dyslexia", benefitForm = "WIA", maritalStatus = "Unmarried",
            driversLicence = "B (Auto)", workingContractOrganization = "SBICT", workingContractFunction = "Software Ontwikkelaar",
            workingContractContractType = "Tijdelijk", workingContractFromDate = "01-12-2022", workingContractToDate = "05-01-2025";

            var gender = Gender.Woman;

            // Act
            await Page.GotoAsync($"{Url}Edit/");

            await Page.GetByTestId(nameof(Client.FirstName).ToLower()).FillAsync(firstName);

            await Page.GetByTestId(nameof(Client.Initials).ToLower()).FillAsync(initials);

            await Page.GetByTestId(nameof(Client.LastName).ToLower()).FillAsync(lastname);

            await Page.GetByTestId(nameof(Client.Address.StreetName).ToLower()).FillAsync(streetName);

            await Page.GetByTestId(nameof(Client.Address.HouseNumber).ToLower()).FillAsync(housenumber);

            await Page.GetByTestId(nameof(Client.Address.HouseNumberAddition).ToLower()).FillAsync(houseNumberAddition);

            await Page.GetByTestId(nameof(Client.Address.PostalCode).ToLower()).FillAsync(postalCode);

            await Page.GetByTestId(nameof(Client.Address.Residence).ToLower()).FillAsync(residence);

            await Page.GetByTestId(nameof(Client.TelephoneNumber).ToLower()).FillAsync(telephoneNumber);

            await Page.GetByTestId(nameof(Client.EmailAddress).ToLower()).FillAsync(emailAddress);

            await Page.GetByTestId(nameof(Client.DateOfBirth).ToLower()).FillAsync(dateOfBirth);

            await Page.GetByTestId(nameof(Client.Gender).ToLower()).SelectOptionAsync(new[] { ((int)gender).ToString() });

            await Page.GetByTestId($"{nameof(Client.EmergencyPeople).ToLower()}.{nameof(EmergencyPerson.Name).ToLower()}.1").FillAsync(emergencyPerson1Name);

            await Page.GetByTestId($"{nameof(Client.EmergencyPeople).ToLower()}.{nameof(EmergencyPerson.TelephoneNumber).ToLower()}.1").FillAsync(emergencyPerson1TelephoneNumber);

            await Page.GetByTestId($"{nameof(Client.EmergencyPeople).ToLower()}.add").ClickAsync();

            await Page.GetByTestId($"{nameof(Client.EmergencyPeople).ToLower()}.{nameof(EmergencyPerson.Name).ToLower()}.2").FillAsync(emergencyPerson2Name);

            await Page.GetByTestId($"{nameof(Client.EmergencyPeople).ToLower()}.{nameof(EmergencyPerson.TelephoneNumber).ToLower()}.2").FillAsync(emergencyPerson2TelephoneNumber);

            await Page.GetByTestId(nameof(Client.Remarks).ToLower()).FillAsync(remark);

            await Page.GetByTestId(nameof(Client.Diagnoses).ToLower()).SelectOptionAsync(new[] { diagnosis1 });
            await Page.GetByTestId($"{nameof(Client.Diagnoses).ToLower()}.add").ClickAsync();

            await Page.GetByTestId(nameof(Client.Diagnoses).ToLower()).SelectOptionAsync(new[] { diagnosis2 });
            await Page.GetByTestId($"{nameof(Client.Diagnoses).ToLower()}.add").ClickAsync();

            await Page.GetByTestId(nameof(Client.BenefitForms).ToLower()).SelectOptionAsync(new[] { benefitForm });
            await Page.GetByTestId($"{nameof(Client.BenefitForms).ToLower()}.add").ClickAsync();

            await Page.GetByTestId(nameof(Client.MaritalStatus).ToLower()).SelectOptionAsync(new[] { maritalStatus });

            await Page.GetByTestId(nameof(Client.DriversLicences).ToLower()).SelectOptionAsync(new[] { driversLicence });
            await Page.GetByTestId($"{nameof(Client.DriversLicences).ToLower()}.add").ClickAsync();

            await Page.GetByTestId($"{nameof(Client.WorkingContracts).ToLower()}.{nameof(WorkingContract.OrganizationId).ToLower()}.1").SelectOptionAsync(new[] { workingContractOrganization });
            await Page.GetByTestId($"{nameof(Client.WorkingContracts).ToLower()}.{nameof(WorkingContract.Function).ToLower()}.1").FillAsync(workingContractFunction);
            await Page.GetByTestId($"{nameof(Client.WorkingContracts).ToLower()}.{nameof(WorkingContract.ContractType).ToLower()}.1").SelectOptionAsync(new[] { workingContractContractType });
            await Page.GetByTestId($"{nameof(Client.WorkingContracts).ToLower()}.{nameof(WorkingContract.FromDate).ToLower()}.1").FillAsync(workingContractFromDate);
            await Page.GetByTestId($"{nameof(Client.WorkingContracts).ToLower()}.{nameof(WorkingContract.ToDate).ToLower()}.1").FillAsync(workingContractToDate);

            await Page.GetByTestId("button.save").ClickAsync();
            await Page.GetByTestId("button.confirm").ClickAsync();

            // Assert
            await Expect(Page.GetByText($"{firstName} {lastname}", new() { Exact = true })).ToBeVisibleAsync();
            await Expect(Page.GetByText(emergencyPerson1Name)).ToBeVisibleAsync();
            await Expect(Page.GetByText(diagnosis1)).ToBeVisibleAsync();
            await Expect(Page.GetByText(maritalStatus)).ToBeVisibleAsync();
            await Expect(Page.GetByText(workingContractOrganization)).ToBeVisibleAsync();
            await Expect(Page.GetByText(benefitForm)).ToBeVisibleAsync();
            await Expect(Page.GetByText(emailAddress)).ToBeVisibleAsync();

            _clientId = int.Parse(await Page.GetByTestId("clientid").InnerTextAsync());
        }

        [Test, Order(3)]
        public async Task UpdateClient_FillInClientDataAndSave_ClientShouldBeUpdated()
        {
            // Arrange
            string firstName = "Piedro", lastname = "Doe";

            // Act
            await Page.GotoAsync($"{Url}Edit/{_clientId}");
            await Task.Delay(TimeSpan.FromSeconds(2));

            await Page.GetByTestId(nameof(Client.FirstName).ToLower()).FillAsync(firstName);

            await Page.GetByTestId("button.save").ClickAsync();
            await Page.GetByTestId("button.confirm").ClickAsync();

            // Assert
            await Expect(Page.GetByText($"{firstName} {lastname}", new() { Exact = true })).ToBeVisibleAsync();
        }

        [Test, Order(4)]
        public async Task DeactivateClient_Client_ShouldBeShownInSearchResults()
        {
            // Arrange
            var url = $"{Url}{_clientId}";
            string firstName = "Piedro", lastname = "Doe";

            // Act
            await Page.GotoAsync($"{Url}{_clientId}");
            await Page.GetByTestId("button.deactivate").ClickAsync();
            await Page.GetByTestId("button.confirmok").ClickAsync();
            await Page.GetByTestId("button.confirmdeactivated").ClickAsync();
            await Page.GotoAsync($"{Url}{_clientId}");

            // Assert
            await Expect(Page.GetByText($"{firstName} {lastname}", new() { Exact = true })).ToBeHiddenAsync();
        }
    }
}
