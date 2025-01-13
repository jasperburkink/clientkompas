using Application.Common.Mappings;
using Application.Common.Models;

using AutoMapper;
using FluentAssertions;

namespace EmailModule.FunctionalTests
{
    public class Tests
    {
        private EmailService _emailService;

        [SetUp]
        public void Setup()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }).CreateMapper();
            _emailService = new EmailService(mapper);
        }

        [Test]
        public async Task Handle_SendEmailAsync_ShouldSendEmail()
        {
            // Arrange
            var id = Guid.NewGuid();

            EmailMessageDto message = new()
            {
                Id = id,
                Subject = "Unit Test E-Mail",
                Recipients = new List<string> { "ontwikkelaar@clientkompas.nl" },
                Body = ""
            };

            // Act
            await _emailService.SendEmailAsync(message, "TestTemplate", new { Title = "Test Bericht" });
            var mailmessage = _emailService.MailMessagesSent.Find(sentMessage => sentMessage.Item1.Id == id);

            // Assert
            mailmessage.Should().NotBeNull();
        }
    }
}
