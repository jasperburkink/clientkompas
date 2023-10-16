using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.Diagnoses.Commands.CreateDiagnosis
{
    public record CreateDiagnosisCommand : IRequest<Diagnosis>
        {
            public int Id { get; init; } 
            public string Name { get; init; }
        }
        public class CreateDiagnosisCommandHandler : IRequestHandler<CreateDiagnosisCommand, Diagnosis>
        {
        private readonly IUnitOfWork _unitOfWork;
            public CreateDiagnosisCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<Diagnosis> Handle(CreateDiagnosisCommand request, CancellationToken cancellationToken)
            {
                var diagnosis = new Diagnosis
                {
                    Id = request.Id,
                    Name = request.Name
                };
                diagnosis.AddDomainEvent(new DiagnosisCreatedEvent(diagnosis));
                await _unitOfWork.DiagnosisRepository.InsertAsync(diagnosis, cancellationToken);
                await _unitOfWork.SaveAsync(cancellationToken);
                return diagnosis;
            }
        }
    }
