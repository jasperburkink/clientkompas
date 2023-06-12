using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Domain
{
    public class EmergencyPerson
    {
        public int Id { get; set; }

        public virtual Client Client { get; set; }

        public string Name { get; set; }

        public string TelephoneNumber { get; set; }
    }
}
