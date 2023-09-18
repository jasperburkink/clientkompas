using Application.Common.Mappings;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Clients.Queries.GetClients
{
    public class WorkingContractDto : IMapFrom<WorkingContract>
    {
        public string CompanyName { get; set; }

        public string Function { get; set; }

        public string ContractType { get; set; }

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }
    }
}
