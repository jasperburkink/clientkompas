using Domain.Common;

namespace Domain.CVS.Domain
{
    public class Diagnosis : BaseAuditableEntity
    {
        public string Name { get; set; }

        public ICollection<Client> Clients { get; } = Array.Empty<Client>();
    }
}
