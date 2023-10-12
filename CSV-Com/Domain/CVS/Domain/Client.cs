using Domain.Common;
using Domain.CVS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Domain
{
    public class Client : BaseAuditableEntity
    {
        public int IdentificationNumber { get; set; }

        public string FirstName { get; set; }

        public string Initials { get; set; }

        public string PrefixLastName { get; set; }

        public string LastName{ get; set; }

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

        public virtual ICollection<DriversLicence> DriversLicences { get; set; } = new List<DriversLicence>();

        public virtual ICollection<EmergencyPerson> EmergencyPeople { get; set; } = new List<EmergencyPerson>();

        public List<Diagnosis> Diagnoses { get; } = new();
        //public virtual ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();

        public BenefitForm BenefitForm { get; set; }

        public virtual ICollection<WorkingContract> WorkingContracts { get; set; } = new List<WorkingContract>();

        public string Remarks { get; set; }
    }
}
