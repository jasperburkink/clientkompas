using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Domain
{
    public class Client
    {
        public int ClientId { get; set; }

        public int BSNNumber { get; set; }

        public string DisplayName { get; set; }

        public string Initials { get; set; }

        public string Infix { get; set; }

        public string LastName{ get; set; }

        public string StreetName { get; set; }

        public int HouseNumber { get; set; }

        public string HouseNumberAddition { get; set; }

        public string PostalCode { get; set; }

        public string Residence { get; set; }

        public string TelephoneNumber { get; set; }

        public string MobileNumber { get; set; }

        public string EmailAddress { get; set; }                
    }
}
