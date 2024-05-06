using Domain.Common;
using Domain.CVS.Enums;
using Domain.CVS.ValueObjects;

namespace Domain.CVS.Domain
{
    public class Client : BaseAuditableEntity
    {
        public string FirstName { get; set; }

        public string Initials { get; set; }

        public string PrefixLastName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get => string.Join(' ', (new string[] { FirstName, PrefixLastName, LastName }).Where(fv => !string.IsNullOrEmpty(fv)).Select(s => s.Trim()));
            set => _ = value;
        }

        public Gender Gender { get; set; }

        public Address Address { get; set; }

        public string TelephoneNumber { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string EmailAddress { get; set; }

        public MaritalStatus? MaritalStatus { get; set; }

        public List<DriversLicence> DriversLicences { get; set; } = new();

        public List<EmergencyPerson> EmergencyPeople { get; set; } = new();

        public List<Diagnosis> Diagnoses { get; set; } = new();

        public List<BenefitForm> BenefitForms { get; set; } = new();

        public List<WorkingContract> WorkingContracts { get; set; } = new();

        public DateTime? DeactivationDateAndTime { get; set; }

        public string Remarks { get; set; }
    }
}
