using Domain.Common;

namespace Domain.CVS.Domain
{
    public class EmergencyPerson : BaseAuditableEntity
    {
        public Client Client { get; set; }

        public string Name { get; set; }

        public string TelephoneNumber { get; set; }
    }
}
