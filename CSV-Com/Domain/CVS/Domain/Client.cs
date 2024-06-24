using System.Text.RegularExpressions;
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
            get
            {
                var fullName = string.Join(' ', (new string[] { FirstName, PrefixLastName, LastName })
                .Where(fv => !string.IsNullOrEmpty(fv))
                .Select(s => s.Trim()));

                return Regex.Replace(fullName, @"\s+", " ");
            }
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

        public DateTime? DeactivationDateTime { get; private set; }

        public string Remarks { get; set; }

        public DateTime Deactivate(DateTime deactivationDateTime)
        {
            // TODO: Maybe add a check to see if the client is already deactivated return result via result pattern.

            var dateTime = deactivationDateTime;
            DeactivationDateTime = dateTime;
            return dateTime;
        }
    }
}
