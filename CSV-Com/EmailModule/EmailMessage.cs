using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;

namespace EmailModule
{
    public class EmailMessage : IMapFrom<EmailMessageDto>
    {
        public required Guid Id { get; set; }

        public required List<string> Recipients { get; set; } = new();

        public List<string> Attachments { get; set; } = new();

        public string Subject { get; set; }

        public string Body { get; set; }

        public void AddAttachment(string attachment)
        {
            Attachments.Add(attachment);
        }

        public void Mapping(Profile profile)
        {

            profile.CreateMap<EmailMessageDto, EmailMessage>();
        }

        public bool IsDuplicateEmail(EmailMessageDto email)
        {
            return email.Id == Id;
        }

    }
}
