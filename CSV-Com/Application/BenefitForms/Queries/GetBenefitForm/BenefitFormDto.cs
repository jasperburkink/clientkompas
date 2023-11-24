using Application.Common.Mappings;
using Domain.CVS.Domain;


namespace Application.BenefitForms.Queries.GetBenefitForm
{
    public class BenefitFormDto : IMapFrom<BenefitForm>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
