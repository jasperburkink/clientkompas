using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Diagnoses.Queries.GetDiagnosis
{
    [Authorize(Policy = Policies.DiagnosisRead)]
    public record GetDiagnosisQuery : IRequest<IEnumerable<DiagnosisDto>> { }
    public class GetDiagnosisQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetDiagnosisQuery, IEnumerable<DiagnosisDto>>
    {
        public async Task<IEnumerable<DiagnosisDto>> Handle(GetDiagnosisQuery request, CancellationToken cancellationToken)
        {
            return (await unitOfWork.DiagnosisRepository.GetAsync())
                .AsQueryable()
                .ProjectTo<DiagnosisDto>(mapper.ConfigurationProvider);
        }
    }
}
