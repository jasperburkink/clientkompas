using Domain.Common;

namespace Domain.CVS.Domain
{
    public class BenefitForm: BaseAuditableEntity
    {


        public string Name { get; set; }

        public ICollection<Client> Client { get; set; } = Array.Empty<Client>();

    }
}
