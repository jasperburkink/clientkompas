using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Domain
{
    public class DriversLicence : BaseAuditableEntity
    {
        

        public string Category { get; set; }

        public string Description { get; set; }

        public List<Client> Clients { get; } = new();

       
    }

  
}
