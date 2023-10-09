using Application.Clients.Commands.CreateClient;
using Application.Clients.Queries.GetClients;
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

                var maritalStatus = await _unitOfWork.MaritalStatusRepository.GetByIDAsync(request.Id, cancellationToken);
                if (maritalStatus == null)
                {
                    throw new NotFoundException(nameof(MaritalStatus), request.Id);
                }

                await _unitOfWork.MaritalStatusRepository.DeleteAsync(maritalStatus);

                await _unitOfWork.SaveAsync(cancellationToken);

                return maritalStatus.Id;
            }
        }
}
