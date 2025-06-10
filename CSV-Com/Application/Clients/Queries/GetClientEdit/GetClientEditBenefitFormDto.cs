using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.Clients.Queries.GetClientEdit
{
    public class GetClientEditBenefitFormDto : IMapFrom<BenefitForm>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
