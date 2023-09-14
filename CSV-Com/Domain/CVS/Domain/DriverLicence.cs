using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Domain
{
    internal class DriverLicence : BaseAuditableEntity
    {
        public string Code { get; set; }

        public string Omschrijving { get; set; }
    }
}
