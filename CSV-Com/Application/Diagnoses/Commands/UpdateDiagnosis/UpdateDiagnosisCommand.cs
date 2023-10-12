using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Diagnoses.Commands.UpdateDiagnosis
{
    public record UpdateDiagnosisCommand : IRequest<int>
        {
            public int Id { get; init; }
            public string Name { get; set; }

        }

        public class UpdateDiagnosisCommandHandler : IRequestHandler<UpdateDiagnosisCommand, int>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateDiagnosisCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<int> Handle(UpdateDiagnosisCommand request, CancellationToken cancellationToken)
            {
                var diagnosis = await _unitOfWork.DiagnosisRepository.GetByIDAsync(request.Id, cancellationToken);
                if (diagnosis == null)
                {
                    throw new NotFoundException(nameof(Diagnosis), request.Id);
                }

            diagnosis.Name = request.Name;


                await _unitOfWork.SaveAsync(cancellationToken);

                return diagnosis.Id;
            }
        }
    }