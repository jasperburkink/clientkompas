using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.BenefitForms.Queries.GetBenefitForm
{
    [Authorize(Policy = Policies.BenefitFormRead)]
    public record GetBenefitFormQuery : IRequest<IEnumerable<BenefitFormDto>> { }

    public class GetBenefitFormQueryHandler : IRequestHandler<GetBenefitFormQuery, IEnumerable<BenefitFormDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetBenefitFormQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BenefitFormDto>> Handle(GetBenefitFormQuery request, CancellationToken cancellationToken)
        {
            return (await _unitOfWork.BenefitFormRepository.GetAsync())
               .AsQueryable()
               .ProjectTo<BenefitFormDto>(_mapper.ConfigurationProvider);
        }
    }
}
