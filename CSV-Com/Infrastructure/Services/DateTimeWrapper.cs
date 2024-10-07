using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    public class DateTimeWrapper : IDateTime
    {
        public DateTime Now => DateTime.Now;

        public DateTime GetUtcNow()
        {
            return DateTime.Now;
        }
    }
}
