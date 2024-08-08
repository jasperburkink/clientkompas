using System.Text.RegularExpressions;
using Domain.Common;
using Domain.CVS.Enums;
using Domain.CVS.ValueObjects;

namespace Domain.CVS.Domain
{
    public class Client : BaseAuditableEntity
    {
        public required string FirstName { get; set; }

        public required string Initials { get; set; }

        public string? PrefixLastName { get; set; }

        public required string LastName { get; set; }

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

        public required Gender Gender { get; set; }

        public required Address Address { get; set; }

        public required string TelephoneNumber { get; set; }

        public required DateOnly DateOfBirth { get; set; }

        public required string EmailAddress { get; set; }

        public MaritalStatus? MaritalStatus { get; set; }

        public required bool IsInTargetGroupRegister { get; set; }

        public List<DriversLicence> DriversLicences { get; set; } = new();

        public List<EmergencyPerson> EmergencyPeople { get; set; } = new();

        public List<Diagnosis> Diagnoses { get; set; } = new();

        public List<BenefitForm> BenefitForms { get; set; } = new();

        public List<WorkingContract> WorkingContracts { get; set; } = new();

        public DateTime? DeactivationDateTime { get; private set; }

        public string? Remarks { get; set; }

        public DateTime Deactivate(DateTime deactivationDateTime)
        {
            if (!DeactivationDateTime.HasValue)
            {
                DeactivationDateTime = deactivationDateTime;
            }

            return DeactivationDateTime.Value;
        }
    }
}
