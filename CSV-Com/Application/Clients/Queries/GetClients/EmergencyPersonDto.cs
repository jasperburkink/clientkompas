using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.Clients.Queries.GetClients
{
    public class EmergencyPersonDto : IMapFrom<EmergencyPerson>
    {
        public string Name { get; set; }

        public string TelephoneNumber { get; set; }
    }
}
