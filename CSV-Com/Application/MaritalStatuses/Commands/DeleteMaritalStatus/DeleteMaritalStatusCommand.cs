using Application.Clients.Commands.CreateClient;
using Application.Clients.Queries.GetClients;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using AutoMapper.QueryableExtensions;
using Domain.CVS.Domain;
using Domain.CVS.Events;
using FluentValidation.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.MaritalStatuses.Commands.DeleteMaritalStatus
{
        public record DeleteMaritalStatusCommand : IRequest<int>
        {
            public int Id { get; init; }
        }
        public class DeleteMaritalStatusCommandHandler : IRequestHandler<DeleteMaritalStatusCommand, int>
        {
            private readonly IUnitOfWork _unitOfWork;
            public DeleteMaritalStatusCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<int> Handle(DeleteMaritalStatusCommand request, CancellationToken cancellationToken)
            {
                // Check if maritalstatus exists in the database
                var maritalStatus = await _unitOfWork.MaritalStatusRepository.GetByIDAsync(request.Id, cancellationToken);
                if (maritalStatus == null)
                {
                    throw new NotFoundException(nameof(MaritalStatus), request.Id);
                }

                // Check if there's any client that uses the maritalstatus
                var clients = await _unitOfWork.ClientRepository.GetAsync(c => c.MaritalStatus.Id.Equals(request.Id));

                if(clients.Any())
                {
                    throw new DomainObjectInUseExeption(nameof(MaritalStatus), request.Id, nameof(Client), clients.Select(c => (object)c.Id));
                }
            
                await _unitOfWork.MaritalStatusRepository.DeleteAsync(maritalStatus);
                await _unitOfWork.SaveAsync(cancellationToken);
                return maritalStatus.Id;
            }
        }
}
