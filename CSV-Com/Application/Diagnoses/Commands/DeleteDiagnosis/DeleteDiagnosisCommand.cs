using Application.Diagnoses.Commands.CreateDiagnosis;
using Application.Diagnoses.Queries.GetDiagnosis;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using AutoMapper.QueryableExtensions;
using Domain.CVS.Domain;
using Domain.CVS.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.Diagnoses.Commands.DeleteDiagnosis
{
    public record DeleteDiagnosisCommand : IRequest<Diagnosis>
        {
            public int Id { get; init; }
        }
        public class DeleteDiagnosisCommandHandler : IRequestHandler<DeleteDiagnosisCommand, Diagnosis>
        {
            private readonly IUnitOfWork _unitOfWork;
            public DeleteDiagnosisCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<Diagnosis> Handle(DeleteDiagnosisCommand request, CancellationToken cancellationToken)
            {
                var diagnosis = await _unitOfWork.DiagnosisRepository.GetByIDAsync(request.Id, cancellationToken);
                if (diagnosis == null)
                {
                    throw new NotFoundException(nameof(Diagnosis), request.Id);
                }
                await _unitOfWork.DiagnosisRepository.DeleteAsync(diagnosis);
                await _unitOfWork.SaveAsync(cancellationToken);
                return diagnosis;
            }
        }
    }

