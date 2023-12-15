using Domain.Common;
using Domain.CVS.Enums;

namespace Domain.CVS.Domain
{
    public class Client : BaseAuditableEntity
    {
        public int IdentificationNumber { get; set; }

        public string FirstName { get; set; }

        public string Initials { get; set; }

        public string PrefixLastName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; private set; }

        public Gender Gender { get; set; }

        public string StreetName { get; set; }

        public int HouseNumber { get; set; }

        public string HouseNumberAddition { get; set; }

        public string PostalCode { get; set; }

        public string Residence { get; set; }

        public string TelephoneNumber { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string EmailAddress { get; set; }

        public MaritalStatus MaritalStatus { get; set; }

        public List<DriversLicence> DriversLicences { get; set; } = new();

        public virtual ICollection<EmergencyPerson> EmergencyPeople { get; set; } = new List<EmergencyPerson>();

        public List<Diagnosis> Diagnoses { get; } = new();

        public BenefitForm BenefitForm { get; set; }

        public virtual ICollection<WorkingContract> WorkingContracts { get; set; } = new List<WorkingContract>();

        public string Remarks { get; set; }

        public Client()
        {

            // TODO: onderstaande werkt nog niet. Dit moet ergens anders de waarde verschijnt niet in de database.
            FullName = string.Join(' ', FirstName, string.IsNullOrEmpty(PrefixLastName) ? "" : PrefixLastName, LastName);
        }
    }
}
