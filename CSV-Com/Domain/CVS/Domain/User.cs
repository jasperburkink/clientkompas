using Domain.Common;

namespace Domain.CVS.Domain
{
    public class User : BaseAuditableEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }
    }
}
