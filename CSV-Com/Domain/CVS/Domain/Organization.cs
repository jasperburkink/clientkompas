using Domain.Common;
using Domain.CVS.ValueObjects;

namespace Domain.CVS.Domain
{
    public class Organization : BaseAuditableEntity
    {
        public string CompanyName { get; set; }

        public Address VisitAddress { get; set; }
        public Address InvoiceAddress { get; set; }
        public Address PostAddress { get; set; }

        public string ContactPersonName { get; set; }
        public string ContactPersonFunction { get; set; }
        public string ContactPersonTelephoneNumber { get; set; }
        public string ContactPersonMobilephoneNumber { get; set; }
        public string ContactPersonEmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Website { get; set; }

        public string EmailAddress { get; set; }

        public string KVKNumber { get; set; }

        public string BTWNumber { get; set; }

        public List<WorkingContract> WorkingContracts { get; set; } = new();

        public string IBANNumber { get; set; }

        public string BIC { get; set; }
    }
}
