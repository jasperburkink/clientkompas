using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using MediatR;

namespace Application.Diagnoses.Commands.DeleteDiagnosis
{
    public record DeleteDiagnosisCommand : IRequest
        {
            public int Id { get; init; }
        }

        public class DeleteDiagnosisCommandHandler : IRequestHandler<DeleteDiagnosisCommand>
        {
            private readonly IUnitOfWork _unitOfWork;
            public DeleteDiagnosisCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task Handle(DeleteDiagnosisCommand request, CancellationToken cancellationToken)
            {
                var diagnosis = await _unitOfWork.DiagnosisRepository.GetByIDAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Diagnosis), request.Id);

                await _unitOfWork.DiagnosisRepository.DeleteAsync(diagnosis);

                await _unitOfWork.SaveAsync(cancellationToken);
            }
        }
    }

