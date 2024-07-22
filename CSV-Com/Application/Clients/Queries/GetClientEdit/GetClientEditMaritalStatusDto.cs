using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.Clients.Queries.GetClientEdit
{
    public class GetClientEditMaritalStatusDto : IMapFrom<MaritalStatus>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
