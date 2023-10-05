using Domain.Common;
using Domain.CVS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Domain
{
    public class MaritalStatus : BaseAuditableEntity
    {


        public string Name { get; set; }

        public virtual ICollection<Client> Client { get; set; } = new List<Client>();

    }
}
