using System.Globalization;
using Application.CoachingPrograms.Queries.GetCoachingProgram;
using Application.Common.Interfaces;
using Application.Common.Resources;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using FluentAssertions;
using Microsoft.Playwright;
using TestData;
using TestData.CoachingProgram;

namespace WebApp.FunctionalTests.Pages.CoachingProgram
{
    [TestFixture]
    public class CoachingProgramEditorTests : PageTest
    {
        private const string Url = Constants.Url + "Clients/";
        private int _clientId;

        private readonly string _firstName = "John", _initials = "J", _lastname = "Doe", _streetName = "Dorpstraat", _housenumber = "4",
            _houseNumberAddition = "a", _postalCode = "1234 AB", _residence = "Utrecht", _telephoneNumber = "0123456789",
            _emailAddress = FakerConfiguration.Faker.Person.Email, _dateOfBirth = "02-08-1989", _emergencyPerson1Name = "Jane Doe",
            _emergencyPerson1TelephoneNumber = "0987654321", _emergencyPerson2Name = "Willem Doe", _emergencyPerson2TelephoneNumber = "0654789123",
            _remark = "Wie is John Doe?", _diagnosis1 = TestData.Constants.DIAGNOSIS_OPTIONS.First(), _diagnosis2 = TestData.Constants.DIAGNOSIS_OPTIONS.Take(Range.StartAt(2)).First(),
            _benefitForm = TestData.Constants.BENEFITFORM_OPTIONS.First(), _maritalStatus = TestData.Constants.MARITALSTATUSES_OPTIONS.First(),
            _workingContractFunction = "Software Ontwikkelaar",
            _workingContractContractType = "Tijdelijk", _workingContractFromDate = "01-12-2022", _workingContractToDate = "05-01-2023",
            _title = "Traject voor John", _ordernumber = "XEP642", _beginDate = "01-01-2020", _endDate = "24-03-2024";
        private readonly Gender _gender = Gender.Woman;
        private readonly CoachingProgramType _coachingProgramType = CoachingProgramType.PGB;
        private Domain.CVS.Domain.CoachingProgram _coachingProgram;
        private readonly IResourceMessageProvider _resourceMessageProvider = new ResourceMessageProvider(CultureInfo.CurrentUICulture);

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

            var random = new Random();
            var index = random.Next(TestData.Constants.DRIVERSLICENCE_CATEGORIES.Count());
            var driversLicence = $"{TestData.Constants.DRIVERSLICENCE_CATEGORIES.ElementAt(index)} ({TestData.Constants.DRIVERSLICENCE_CATEGORIES_DESCRIPTIONS.ElementAt(index)})";

            await Page.GetByTestId(nameof(Domain.CVS.Domain.Client.DriversLicences).ToLower()).SelectOptionAsync(new[] { driversLicence });
            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.DriversLicences).ToLower()}.add").ClickAsync();

            await Page.GetByTestId($"{nameof(Domain.CVS.Domain.Client.WorkingContracts).ToLower()}.{nameof(WorkingContract.OrganizationId).ToLower()}.1").SelectOptionAsync(new[] { new SelectOptionValue() { Index = 1 } });
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
            await Expect(Page.GetByText(_benefitForm)).ToBeVisibleAsync();
            await Expect(Page.GetByText(_emailAddress)).ToBeVisibleAsync();

            _clientId = int.Parse(await Page.GetByTestId("clientid").InnerTextAsync());
        }

        [Skip]
        [Ignore("Playwright does not work in the pipeline")]
        //[Test, Order(2)]
        public async Task CreateCoachingProgram_FillInCoachingProgramDataAndSave_ClientShouldBeAdded()
        {
            // Arrange
            var url = $"{Url}{_clientId}/coachingprogram-editor/0";
            var fullName = $"{_firstName} {_lastname}";

            ITestDataGenerator<Domain.CVS.Domain.CoachingProgram> testDataGenerator = new CoachingProgramDataGenerator();
            _coachingProgram = testDataGenerator.Create();

            // Act
            await Page.GotoAsync(url);

            await Page.GetByText(fullName).ElementHandleAsync(); // Wait until client is loaded

            var elementFullName = await Page.GetByTestId("client-fullname").ElementHandleAsync();
            var fullNameClient = await elementFullName.TextContentAsync();

            // Act
            await Page.GetByTestId(nameof(Domain.CVS.Domain.CoachingProgram.Title).ToLower()).FillAsync(_coachingProgram.Title);
            await Page.GetByTestId(nameof(Domain.CVS.Domain.CoachingProgram.OrderNumber).ToLower()).FillAsync(_coachingProgram.OrderNumber);
            await Page.GetByTestId(nameof(Domain.CVS.Domain.CoachingProgram.Organization).ToLower()).SelectOptionAsync(new[] { new SelectOptionValue() { Index = 1 } });
            await Page.GetByTestId(nameof(Domain.CVS.Domain.CoachingProgram.CoachingProgramType).ToLower()).SelectOptionAsync(new[] { new SelectOptionValue() { Value = ((int)_coachingProgram.CoachingProgramType).ToString() } });
            await Page.GetByTestId(nameof(Domain.CVS.Domain.CoachingProgram.BeginDate).ToLower()).FillAsync(_coachingProgram.BeginDate.ToString("dd-MM-yyyy"));
            await Page.GetByTestId(nameof(Domain.CVS.Domain.CoachingProgram.EndDate).ToLower()).FillAsync(_coachingProgram.EndDate.ToString("dd-MM-yyyy"));

            await Page.GetByTestId(nameof(Domain.CVS.Domain.CoachingProgram.BudgetAmmount).ToLower()).FillAsync(_coachingProgram.BudgetAmmount.ToString().Replace('.', ','));
            await Page.GetByTestId(nameof(Domain.CVS.Domain.CoachingProgram.HourlyRate).ToLower()).FillAsync(_coachingProgram.HourlyRate.ToString().Replace('.', ','));

            await Page.GetByTestId("button.save").ClickAsync();
            await Page.GetByTestId("button.confirm").ClickAsync();

            // Assert
            fullNameClient.Should().Be(fullName);
        }

        [Skip]
        [Ignore("Playwright does not work in the pipeline")]
        //[Test, Order(3)]
        public async Task ClientCoachingProgram_ShowDetails_CoachingProgramShouldBeVisible()
        {
            // Arrange
            var url = $"{Url}{_clientId}";
            var fullName = $"{_firstName} {_lastname}";
            var coachingProgramName = _resourceMessageProvider.GetMessage(typeof(GetCoachingProgramDto), Enum.GetName(_coachingProgram.CoachingProgramType));
            var culture = new CultureInfo("nl-NL");

            // Act
            await Page.GotoAsync(url);
            await Page.GetByTestId(nameof(Domain.CVS.Domain.CoachingProgram).ToLower()).SelectOptionAsync(new[] { _coachingProgram.Title });

            // Assert
            await Expect(Page.GetByTestId("coachingprogram-begindate")).ToContainTextAsync(_coachingProgram.BeginDate.ToString("dd-MM-yyyy"));
            await Expect(Page.GetByTestId("coachingprogram-enddate")).ToContainTextAsync(_coachingProgram.EndDate.ToString("dd-MM-yyyy"));
            await Expect(Page.GetByTestId("coachingprogram-coachingprogramtype")).ToContainTextAsync(coachingProgramName);
            await Expect(Page.GetByTestId("coachingprogram-clientfullname")).ToContainTextAsync(fullName);
            await Expect(Page.GetByTestId("coachingprogram-title")).ToContainTextAsync(_coachingProgram.Title);
            if (!string.IsNullOrEmpty(_coachingProgram.OrderNumber))
            {
                await Expect(Page.GetByTestId("coachingprogram-ordernumber")).ToContainTextAsync(_coachingProgram.OrderNumber);
            }
            if (_coachingProgram.BudgetAmmount.HasValue)
            {
                await Expect(Page.GetByTestId("coachingprogram-budgetammount")).ToContainTextAsync(Math.Round(_coachingProgram.BudgetAmmount.Value, 2).ToString("N", culture));
            }
            await Expect(Page.GetByTestId("coachingprogram-hourlyrate")).ToContainTextAsync(Math.Round(_coachingProgram.HourlyRate, 2).ToString("N", culture));
            await Expect(Page.GetByTestId("coachingprogram-remaininghours")).ToContainTextAsync(Math.Round(_coachingProgram.RemainingHours, 2).ToString("N", culture));
        }
    }
}
