using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;

namespace Application.Clients.Dtos
{
    public class ClientWorkingContractOrganizationDto : IMapFrom<Organization>
    {
        public int Id { get; set; }

        public string OrganizationName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Organization, ClientWorkingContractOrganizationDto>();
        }

        public Organization ToDomainModel(IMapper mapper) // TODO: remove this method when upgrading to .net 8. Automapper issues have been solved in newer versions.
        {
            var converter = new JsonStringEnumConverter();

            var domainModel = JsonSerializer.Deserialize<Organization>(JsonSerializer.Serialize(this), new JsonSerializerOptions()
            {

                Converters = {
                    converter
                }
            });
            return domainModel;
        }
    }
}
