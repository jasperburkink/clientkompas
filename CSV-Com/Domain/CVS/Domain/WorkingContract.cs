using Domain.CVS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Domain
{
    public class WorkingContract
    {
        public int Id { get; set; }

        public virtual Client Client { get; set; }

        public string CompanyName { get; set; }

        public string Function { get; set; }

        public ContractType ContractType { get; set; }

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }
    }
}
