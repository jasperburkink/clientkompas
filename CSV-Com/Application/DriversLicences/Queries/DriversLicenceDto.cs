using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.DriversLicences.Queries
{
    public class DriversLicenceDto : IMapFrom<DriversLicence>
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }
    }
}
