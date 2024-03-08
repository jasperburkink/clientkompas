using System.Text.Json;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;

namespace Application.Clients.Dtos
{
    public class WorkingContractDto : IMapFrom<WorkingContract>
    {
        public string CompanyName { get; set; }

        public string Function { get; set; }

        public ContractType ContractType { get; set; }

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public WorkingContract ToDomainModel(IMapper mapper, Client client)
        {
            var domainModel = JsonSerializer.Deserialize<WorkingContract>(JsonSerializer.Serialize(this));
            domainModel.Client = client;
            return domainModel;
        }
    }
}
