using Domain.Common;
using Domain.CVS.Enums;

namespace Domain.CVS.Domain
{
    public class Client : BaseAuditableEntity
    {
        private string _firstName;
        private string _prefixLastName;
        private string _lastName;

        public int IdentificationNumber { get; set; }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                SetFullName();
            }
        }

        public string Initials { get; set; }

        public string PrefixLastName
        {
            get => _prefixLastName;
            set
            {
                _prefixLastName = value;
                SetFullName();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                SetFullName();
            }
        }

        public string FullName { get; set; }

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

        private void SetFullName()
        {
            var fieldValues = new string[] { FirstName, PrefixLastName, LastName };
            FullName = string.Join(' ', fieldValues.Where(fv => !string.IsNullOrEmpty(fv)).Select(s => s.Trim()));
        }
    }
}
