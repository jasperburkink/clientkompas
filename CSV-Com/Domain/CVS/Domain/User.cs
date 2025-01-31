using Domain.Common;

namespace Domain.CVS.Domain
{
    public class User : BaseAuditableEntity
    {
        public required string FirstName { get; set; }

        public string? PrefixLastName { get; set; }

        public required string LastName { get; set; }

        public required string EmailAddress { get; set; }

        public required string TelephoneNumber { get; set; }

        public required bool IsDeactivated { get; set; }
    }
}
