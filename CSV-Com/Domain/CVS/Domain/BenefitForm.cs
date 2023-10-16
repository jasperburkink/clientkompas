using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Domain
{
    public class BenefitForm: BaseAuditableEntity
    {


        public string Name { get; set; }

        public virtual ICollection<Client> Client { get; set; } = new List<Client>();

    }
}
