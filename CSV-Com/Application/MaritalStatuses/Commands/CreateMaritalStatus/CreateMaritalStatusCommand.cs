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

namespace Application.MaritalStatuses.Commands.CreateMaritalStatus
{
    public record CreateMaritalStatusCommand : IRequest<int>
    {
        public int Id { get; init; }

        public string Name { get; init; }

    }

    public class CreateMaritalStatusCommandHandler : IRequestHandler<CreateMaritalStatusCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateMaritalStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateMaritalStatusCommand request, CancellationToken cancellationToken)
        {
            var maritalStatus = new MaritalStatus
            {
                Id = request.Id,
                Name = request.Name
               
            };

            maritalStatus.AddDomainEvent(new MaritalStatusCreatedEvent(maritalStatus));

            await _unitOfWork.MaritalStatusRepository.InsertAsync(maritalStatus, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return maritalStatus.Id;
        }
    }
}
