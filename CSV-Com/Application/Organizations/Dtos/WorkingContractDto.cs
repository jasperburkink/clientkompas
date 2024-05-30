using System.Text.Json;
using AutoMapper;
using Domain.CVS.Domain;
using Application.Common.Mappings;

namespace Application.Organizations.Dtos
{
    public class WorkingContractDto : IMapFrom<WorkingContract>
    {
        public int Id { get; set; }

        public string Function { get; set; }

        public int ContractType { get; set; }

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public int ClientId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkingContract, WorkingContractDto>()
                .ForMember(dto => dto.ClientId, opt => opt.MapFrom(src => src.Client.Id));
        }

        public WorkingContract ToDomainModel(IMapper mapper, Organization organization, Client client)
        {
            var domainModel = JsonSerializer.Deserialize<WorkingContract>(JsonSerializer.Serialize(this));
            domainModel.Organization = organization;
            domainModel.Client = client;
            return domainModel;
        }
    }
}
