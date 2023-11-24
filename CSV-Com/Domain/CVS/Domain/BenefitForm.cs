using Domain.Common;

namespace Domain.CVS.Domain
{
    public class BenefitForm : BaseAuditableEntity
    {


        public string Name { get; set; }

        public List<Client> Client { get; set; }
        new = List<Client>;

    }
}
