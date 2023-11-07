using Application.Common.Interfaces.CVS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Diagnoses.Queries.GetDiagnosis
{
    public record GetDiagnosisQuery : IRequest<IEnumerable<DiagnosisDto>> { }
        public class GetDiagnosisQueryHandler : IRequestHandler<GetDiagnosisQuery, IEnumerable<DiagnosisDto>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public GetDiagnosisQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<IEnumerable<DiagnosisDto>> Handle(GetDiagnosisQuery request, CancellationToken cancellationToken)
            {
                return (await _unitOfWork.DiagnosisRepository.GetAsync())
                   .AsQueryable()
                   .ProjectTo<DiagnosisDto>(_mapper.ConfigurationProvider);
            }
        }
    }