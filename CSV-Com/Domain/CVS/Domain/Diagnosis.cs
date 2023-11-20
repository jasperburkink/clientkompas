using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Domain
{
    public class Diagnosis : BaseAuditableEntity
    {
        public string Name { get; set; }

        public ICollection<Client> Clients { get; } = Array.Empty<Client>();
    }
}
