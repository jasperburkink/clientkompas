using Domain.Common;


namespace Domain.CVS.Domain
{
    public class DriversLicence : BaseAuditableEntity
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public List<Client> Clients { get; } = new();
    }
}
