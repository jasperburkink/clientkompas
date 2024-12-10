namespace EmailModule.FunctionalTests
{
    public class Tests
    {
        private EmailService _emailService;

        [SetUp]
        public void Setup()
        {
            _emailService = new EmailService();
        }

        [Test]
        public async Task TestSendEmailAsync()
        {
            // Arrange
            var emailMessage = new EmailMessage
            {
                To = new List<string> { "ontwikkelaar@clientkompas.nl" },
                Subject = "Unit Test E-Mail",
                Body = "Dit is een test E-Mail verzonden uit de unit test"
            };

            var templateName = "TestTemplate";
            var model = new { Title = "Test Bericht" };

            // Act
            await _emailService.SendEmailAsync(emailMessage, templateName, model);

            // Assert
            Assert.Pass();
        }
    }
}
