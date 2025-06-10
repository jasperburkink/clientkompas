using Domain.Common;
using Domain.CVS.Enums;

namespace Domain.CVS.Domain
{
    public class License : BaseAuditableEntity
    {
        public LicenseStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ValidUntil { get; set; }
        public Organization Organization { get; set; }
        public int OrganizationId { get; set; }
        public User LicenseHolder { get; set; }
        public int LicenseHolderId { get; set; }
    }
}
