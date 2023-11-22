using Domain.Common;
using Domain.CVS.Enums;

namespace Domain.CVS.Domain
{
    public class DriversLicence : BaseAuditableEntity
    {
        public virtual Client Client { get; set; }

        public DriversLicenceEnum DriversLicenceCode { get; set; }
    }
}
