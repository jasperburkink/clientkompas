using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;

namespace Application.Clients.Dtos
{
    public class ClientWorkingContractDto : IMapFrom<WorkingContract>
    {
        public int Id { get; set; }

        public string Function { get; set; }

        public int ContractType { get; set; }

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkingContract, ClientWorkingContractDto>();
        }

        public WorkingContract ToDomainModel(IMapper mapper, Client client) // TODO: remove this method when upgrading to .net 8. Automapper issues have been solved in newer versions.
        {
            var converter = new JsonStringEnumConverter();

            var domainModel = JsonSerializer.Deserialize<WorkingContract>(JsonSerializer.Serialize(this), new JsonSerializerOptions()
            {

                Converters = {
                    converter
                }
            });
            domainModel.Client = client;
            return domainModel;
        }
    }
}
