using Domain.CVS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Domain
{
    public class Client
    {
        public int Id { get; set; }

        public int IdentificationNumber { get; set; }

        public string FirstName { get; set; }

        public string Initials { get; set; }

        public string PrefixLastName { get; set; }

        public string LastName{ get; set; }

        public Sex Sex { get; set; }

        public string StreetName { get; set; }

        public int HouseNumber { get; set; }

        public string HouseNumberAddition { get; set; }

        public string PostalCode { get; set; }

        public string Residence { get; set; }

        public string TelephoneNumber { get; set; }

        public string EmailAddress { get; set; }        

        public MaritalStatus MaritalStatus { get; set; }

        public ICollection<DriversLicence> DriversLicence { get; set; }

        public ICollection<EmergencyPerson> EmergencyPeople { get; set; }

        public ICollection<Diagnosis> Diagnoses { get; set; }

        public BenefitForm BenefitForm { get; set; }

        public ICollection<WorkingContract> WorkingContract { get; set; }

        public string Remarks { get; set; }
    }
}
