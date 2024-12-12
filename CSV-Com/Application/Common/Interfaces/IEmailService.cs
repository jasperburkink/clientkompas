using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync<T>(EmailMessageDto messageDto, string templateName, T model);
    }
}
