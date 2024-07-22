using System.Text.Json;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;

namespace Application.Clients.Queries.GetClientEdit
{
    public class GetClientEditEmergencyPersonDto : IMapFrom<EmergencyPerson>
    {
        public string Name { get; set; }

        public string TelephoneNumber { get; set; }

        public EmergencyPerson ToDomainModel(IMapper mapper, Client client)
        {
            var domainModel = JsonSerializer.Deserialize<EmergencyPerson>(JsonSerializer.Serialize(this));
            domainModel.Client = client;
            return domainModel;
        }
    }
}
