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

    public class CreateDiagnosisCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateDiagnosisCommand, DiagnosisDto>
    {
        public async Task<DiagnosisDto> Handle(CreateDiagnosisCommand request, CancellationToken cancellationToken)
        {
            var diagnosis = new Diagnosis
            {
                Name = request.Name
            };

            diagnosis.AddDomainEvent(new DiagnosisCreatedEvent(diagnosis));

            await unitOfWork.DiagnosisRepository.InsertAsync(diagnosis, cancellationToken);

            await unitOfWork.SaveAsync(cancellationToken);

            return mapper.Map<DiagnosisDto>(diagnosis);
        }
    }
}
