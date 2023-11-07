using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.MaritalStatuses.Queries.GetMaritalStatus
{
    public class MaritalStatusDto : IMapFrom<MaritalStatus>
    {
            public int Id { get; set; }

            public string Name { get; set; }  
    }
}
