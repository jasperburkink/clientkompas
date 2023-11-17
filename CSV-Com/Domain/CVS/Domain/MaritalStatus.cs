using Domain.Common;

namespace Domain.CVS.Domain
{
    public class MaritalStatus : BaseAuditableEntity
    {


        public string Name { get; set; }

        public virtual ICollection<Client> Client { get; set; } = new List<Client>();

    }
}
