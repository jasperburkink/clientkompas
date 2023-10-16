using Application.Common.Interfaces.CVS;
using Application.MaritalStatuses.Queries.GetMaritalStatus;
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
using System.Xml;
namespace Application.MaritalStatuses.Commands.CreateMaritalStatus
{
    public record CreateMaritalStatusCommand : IRequest<MaritalStatus>
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
    public class CreateMaritalStatusCommandHandler : IRequestHandler<CreateMaritalStatusCommand, MaritalStatus>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateMaritalStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<MaritalStatus> Handle(CreateMaritalStatusCommand request, CancellationToken cancellationToken)
        {
            var maritalStatus = new MaritalStatus
            {
                Id = request.Id,
                Name = request.Name
            };
            maritalStatus.AddDomainEvent(new MaritalStatusCreatedEvent(maritalStatus));
            await _unitOfWork.MaritalStatusRepository.InsertAsync(maritalStatus, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            return maritalStatus;
        }
    }
}
