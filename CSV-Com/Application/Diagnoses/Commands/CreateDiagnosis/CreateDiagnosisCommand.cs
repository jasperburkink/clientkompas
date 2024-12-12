using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Application.Diagnoses.Queries.GetDiagnosis;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Domain.CVS.Events;

namespace Application.Diagnoses.Commands.CreateDiagnosis
{
    [Authorize(Policy = Policies.DiagnosisManagement)]
    public record CreateDiagnosisCommand : IRequest<DiagnosisDto>
    {
        public string Name { get; init; }
    }

    public class CreateDiagnosisCommandHandler : IRequestHandler<CreateDiagnosisCommand, DiagnosisDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDiagnosisCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DiagnosisDto> Handle(CreateDiagnosisCommand request, CancellationToken cancellationToken)
        {
            var diagnosis = new Diagnosis
            {
                Name = request.Name
            };

            diagnosis.AddDomainEvent(new DiagnosisCreatedEvent(diagnosis));

            await _unitOfWork.DiagnosisRepository.InsertAsync(diagnosis, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return _mapper.Map<DiagnosisDto>(diagnosis);
        }
    }
}
