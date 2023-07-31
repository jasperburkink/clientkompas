using Application.Common.Mappings;
using Domain.CVS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Clients.Queries.GetClients
{
    public class EmergencyPersonDto : IMapFrom<EmergencyPerson>
    {
        public string Name { get; set; }

        public string TelephoneNumber { get; set; }
    }
}
