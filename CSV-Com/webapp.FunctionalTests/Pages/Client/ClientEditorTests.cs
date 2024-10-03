using Domain.CVS.Domain;
using Domain.CVS.Enums;
using FluentAssertions;
using TestData;

namespace WebApp.FunctionalTests.Pages.Client
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ClientEditorTests : PageTest
    {
        // TODO: test mobile

        private const string Url = Constants.Url + "Clients/";
        private int _clientId;

        private readonly string _firstName = "John", _initials = "J", _lastname = "Doe", _streetName = "Dorpstraat", _housenumber = "4",
            _houseNumberAddition = "a", _postalCode = "1234 AB", _residence = "Utrecht", _telephoneNumber = "0123456789",
            _emailAddress = FakerConfiguration.Faker.Person.Email, _dateOfBirth = "02-08-1989", _emergencyPerson1Name = "Jane Doe",
            _emergencyPerson1TelephoneNumber = "0987654321", _emergencyPerson2Name = "Willem Doe", _emergencyPerson2TelephoneNumber = "0654789123",
            _remark = "Wie is John Doe?", _diagnosis1 = "Depression", _diagnosis2 = "Dyslexia", _benefitForm = "WIA", _maritalStatus = "Unmarried",
            _driversLicence = "B (Auto)", _workingContractOrganization = "SBICT", _workingContractFunction = "Software Ontwikkelaar",
            _workingContractContractType = "Tijdelijk", _workingContractFromDate = "01-12-2022", _workingContractToDate = "05-01-2025";
        private readonly Gender _gender = Gender.Woman;

        private readonly string _updatedFirstName = "Piedro";

        [Skip]
        [Ignore("Playwright does not work in the pipeline")]
        //[Test, Order(1)]
        public async Task CreateClient_FillInClientDataAndSave_ClientShouldBeAdded()
        {
            // Arrange
            var url = $"{Url}Edit/";
            var fullName = $"{_firstName} {_lastname}";

            // Act
            await Page.GotoAsync(url);

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.FirstName).ToLower()).FillAsync(_firstName);

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.Initials).ToLower()).FillAsync(_initials);

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.LastName).ToLower()).FillAsync(_lastname);

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.Address.StreetName).ToLower()).FillAsync(_streetName);

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.Address.HouseNumber).ToLower()).FillAsync(_housenumber);

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.Address.HouseNumberAddition).ToLower()).FillAsync(_houseNumberAddition);

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.Address.PostalCode).ToLower()).FillAsync(_postalCode);

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.Address.Residence).ToLower()).FillAsync(_residence);

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.TelephoneNumber).ToLower()).FillAsync(_telephoneNumber);

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.EmailAddress).ToLower()).FillAsync(_emailAddress);

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.DateOfBirth).ToLower()).FillAsync(_dateOfBirth);

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.Gender).ToLower()).SelectOptionAsync(new[] { ((int)_gender).ToString() });

            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.EmergencyPeople).ToLower()}.{nameof(EmergencyPerson.Name).ToLower()}.1").FillAsync(_emergencyPerson1Name);

            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.EmergencyPeople).ToLower()}.{nameof(EmergencyPerson.TelephoneNumber).ToLower()}.1").FillAsync(_emergencyPerson1TelephoneNumber);

            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.EmergencyPeople).ToLower()}.add").ClickAsync();

            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.EmergencyPeople).ToLower()}.{nameof(EmergencyPerson.Name).ToLower()}.2").FillAsync(_emergencyPerson2Name);

            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.EmergencyPeople).ToLower()}.{nameof(EmergencyPerson.TelephoneNumber).ToLower()}.2").FillAsync(_emergencyPerson2TelephoneNumber);

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.Remarks).ToLower()).FillAsync(_remark);

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.Diagnoses).ToLower()).SelectOptionAsync(new[] { _diagnosis1 });
            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.Diagnoses).ToLower()}.add").ClickAsync();

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.Diagnoses).ToLower()).SelectOptionAsync(new[] { _diagnosis2 });
            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.Diagnoses).ToLower()}.add").ClickAsync();

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.BenefitForms).ToLower()).SelectOptionAsync(new[] { _benefitForm });
            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.BenefitForms).ToLower()}.add").ClickAsync();

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.MaritalStatus).ToLower()).SelectOptionAsync(new[] { _maritalStatus });

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.DriversLicences).ToLower()).SelectOptionAsync(new[] { _driversLicence });
            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.DriversLicences).ToLower()}.add").ClickAsync();

            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.WorkingContracts).ToLower()}.{nameof(WorkingContract.OrganizationId).ToLower()}.1").SelectOptionAsync(new[] { _workingContractOrganization });
            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.WorkingContracts).ToLower()}.{nameof(WorkingContract.Function).ToLower()}.1").FillAsync(_workingContractFunction);
            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.WorkingContracts).ToLower()}.{nameof(WorkingContract.ContractType).ToLower()}.1").SelectOptionAsync(new[] { _workingContractContractType });
            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.WorkingContracts).ToLower()}.{nameof(WorkingContract.FromDate).ToLower()}.1").FillAsync(_workingContractFromDate);
            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.WorkingContracts).ToLower()}.{nameof(WorkingContract.ToDate).ToLower()}.1").FillAsync(_workingContractToDate);

            await Page.GetByTestId("button.save").ClickAsync();
            await Page.GetByTestId("button.confirm").ClickAsync();

            // Assert
            var elementFullName = await Page.GetByTestId("client-fullname").ElementHandleAsync();
            var fullNameClient = await elementFullName.TextContentAsync();
            fullNameClient.Should().Be(fullName);

            await Expect(Page.GetByText(_emergencyPerson1Name)).ToBeVisibleAsync();
            await Expect(Page.GetByText(_diagnosis1)).ToBeVisibleAsync();
            await Expect(Page.GetByText(_maritalStatus)).ToBeVisibleAsync();
            await Expect(Page.GetByText(_workingContractOrganization)).ToBeVisibleAsync();
            await Expect(Page.GetByText(_benefitForm)).ToBeVisibleAsync();
            await Expect(Page.GetByText(_emailAddress)).ToBeVisibleAsync();

            _clientId = int.Parse(await Page.GetByTestId("clientid").InnerTextAsync());
        }

        [Skip]
        [Ignore("Playwright does not work in the pipeline")]
        //[Test, Order(2)]
        public async Task GetClient_ShowClientData_ClientDataShouldBeVisible()
        {
            // Arrange
            var fullName = $"{_firstName} {_lastname}";

            var url = $"{Url}{_clientId}";

            // Act
            await Page.GotoAsync(url);

            var elementClientId = await Page.GetByTestId("clientid").ElementHandleAsync();
            var elementFullName = await Page.GetByTestId("client-fullname").ElementHandleAsync();
            var elementEmailAddress = await Page.GetByTestId("client-emailaddress").ElementHandleAsync();

            var clientId = await elementClientId.TextContentAsync();
            var fullNameClient = await elementFullName.TextContentAsync();
            var emailClient = await elementEmailAddress.TextContentAsync();

            // Assert
            clientId.Should().Be(_clientId.ToString());
            fullNameClient.Should().Be(fullName);
            emailClient.Should().Be(_emailAddress);
        }

        [Skip]
        [Ignore("Playwright does not work in the pipeline")]
        //[Test, Order(3)]
        public async Task UpdateClient_FillInClientDataAndSave_ClientShouldBeUpdated()
        {
            // Arrange
            var url = $"{Url}Edit/{_clientId}";

            // Act
            await Page.GotoAsync(url);
            await Task.Delay(TimeSpan.FromSeconds(2)); // NOTE: For strange reasons there's a delay needed. Else an exception will be thrown.

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.FirstName).ToLower()).FillAsync(_updatedFirstName);

            await Page.GetByTestId("button.save").ClickAsync();
            await Page.GetByTestId("button.confirm").ClickAsync();

            // Assert
            await Expect(Page.GetByText($"{_updatedFirstName} {_lastname}", new() { Exact = true })).ToBeVisibleAsync();
        }

        [Skip]
        [Ignore("Playwright does not work in the pipeline")]
        //[Test, Order(4)]
        public async Task DeactivateClient_ClientIsBeingDeactivated_ShouldNotBeShownInSearchResults()
        {
            // Arrange
            var url = $"{Url}{_clientId}";

            // Act
            await Page.GotoAsync(url);
            await Page.GetByTestId("button.deactivate").ClickAsync();
            await Page.GetByTestId("button.confirmok").ClickAsync();
            await Page.GetByTestId("button.confirmdeactivated").ClickAsync();
            await Page.GotoAsync($"{Url}{_clientId}");

            // Assert
            await Expect(Page.GetByText($"{_updatedFirstName} {_lastname}", new() { Exact = true })).ToBeHiddenAsync();
        }
    }
}
