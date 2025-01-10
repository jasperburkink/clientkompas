using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Application.Diagnoses.Queries.GetDiagnosis;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;

namespace Application.Diagnoses.Commands.UpdateDiagnosis
{
    [Authorize(Policy = Policies.DiagnosisManagement)]
    public record UpdateDiagnosisCommand : IRequest<DiagnosisDto>
    {
        public int Id { get; init; }

        public string Name { get; set; }
    }

    public class UpdateDiagnosisCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateDiagnosisCommand, DiagnosisDto>
    {
        public async Task<DiagnosisDto> Handle(UpdateDiagnosisCommand request, CancellationToken cancellationToken)
        {
            var diagnosis = await unitOfWork.DiagnosisRepository.GetByIDAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Diagnosis), request.Id);

            diagnosis.Name = request.Name;

            await unitOfWork.SaveAsync(cancellationToken);

            return mapper.Map<DiagnosisDto>(diagnosis);
        }
    }
}
