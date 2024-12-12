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

    public class UpdateDiagnosisCommandHandler : IRequestHandler<UpdateDiagnosisCommand, DiagnosisDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpdateDiagnosisCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DiagnosisDto> Handle(UpdateDiagnosisCommand request, CancellationToken cancellationToken)
        {
            var diagnosis = await _unitOfWork.DiagnosisRepository.GetByIDAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Diagnosis), request.Id);

            diagnosis.Name = request.Name;

            await _unitOfWork.SaveAsync(cancellationToken);

            return _mapper.Map<DiagnosisDto>(diagnosis);
        }
    }
}
