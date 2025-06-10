using System.Text.Json;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;

namespace Application.DriversLicences.Queries
{
    public class DriversLicenceDto : IMapFrom<DriversLicence>
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public DriversLicence ToDomainModel(IMapper mapper, Client client)
        {
            var domainModel = JsonSerializer.Deserialize<DriversLicence>(JsonSerializer.Serialize(this));
            return domainModel;
        }
    }
}
