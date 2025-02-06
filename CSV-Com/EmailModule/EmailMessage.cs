using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;

namespace EmailModule
{
    public class EmailMessage : IMapFrom<EmailMessageDto>
    {
        public required Guid Id { get; set; }

        public required List<string> Recipients { get; set; } = [];

        public List<string> Attachments { get; set; } = [];

        public required string Subject { get; set; }

        public string? Body { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EmailMessageDto, EmailMessage>();
        }
    }
}
