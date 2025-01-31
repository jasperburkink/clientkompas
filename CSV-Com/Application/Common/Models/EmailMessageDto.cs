namespace Application.Common.Models
{
    public class EmailMessageDto
    {
        public required Guid Id { get; set; }

        public required List<string> Recipients { get; set; } = [];

        public List<string> Attachments { get; set; } = [];

        public string Subject { get; set; }

        public string Body { get; set; }


    }
}

