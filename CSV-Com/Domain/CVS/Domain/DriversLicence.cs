using Domain.CVS.Common;
using Domain.CVS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Domain
{
    public class DriversLicence : BaseAuditableEntity
    {
        public virtual Client Client { get; set; }

        public DriversLicenceEnum DriversLicenceCode { get; set;}
    }
}
