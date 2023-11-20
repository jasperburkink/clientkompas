using Domain.Common;


namespace Domain.CVS.Domain
{
    public class Diagnosis : BaseAuditableEntity
    {
        public virtual Client Client { get; set; }

        public string Name { get; set; }
    }
}
