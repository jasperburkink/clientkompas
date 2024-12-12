namespace Application.Common.Models
{
    public class EmailMessageDto
    {
        public required List<string> To { get; set; } = new();

        public List<string> Attachments { get; set; } = new();

        public string Subject { get; set; }

        public string Body { get; set; }

        public void AddRecipient(string recipient)
        {
            To.Add(recipient);
        }

        public void AddAttachment(string attachment)
        {
            Attachments.Add(attachment);
        }
    }
}
