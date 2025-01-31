using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.BenefitForms.Queries.GetBenefitForm
{
    [Authorize(Policy = Policies.BenefitFormRead)]
    public record GetBenefitFormQuery : IRequest<IEnumerable<BenefitFormDto>> { }

    public class GetBenefitFormQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBenefitFormQuery, IEnumerable<BenefitFormDto>>
    {
        public async Task<IEnumerable<BenefitFormDto>> Handle(GetBenefitFormQuery request, CancellationToken cancellationToken)
        {
            return (await unitOfWork.BenefitFormRepository.GetAsync())
               .AsQueryable()
               .ProjectTo<BenefitFormDto>(mapper.ConfigurationProvider);
        }
    }
}
