using Domain.Common;

namespace Domain.CVS.Domain
{
    public class BenefitForm : BaseAuditableEntity
    {
        public string Name { get; set; }

        public List<Client> Clients { get; } = new List<Client>();

    }
}
