using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.Clients.Dtos
{
    public class EmergencyPersonDto : IMapFrom<EmergencyPerson>
    {
        public string Name { get; set; }

        public string TelephoneNumber { get; set; }
    }
}
