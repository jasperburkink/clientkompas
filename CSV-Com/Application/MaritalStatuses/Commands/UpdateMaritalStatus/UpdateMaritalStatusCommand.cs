using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MaritalStatuses.Commands.UpdateMaritalStatus
{
    public record UpdateMaritalStatusCommand : IRequest<int>
        {
            public int Id { get; init; }
            public string Name { get; set; }
            
        }

        public class UpdateMaritalStatusCommandHandler : IRequestHandler<UpdateMaritalStatusCommand, int>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateMaritalStatusCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<int> Handle(UpdateMaritalStatusCommand request, CancellationToken cancellationToken)
            {
                var maritalStatus = await _unitOfWork.MaritalStatusRepository.GetByIDAsync(request.Id, cancellationToken);
                if (maritalStatus == null)
                {
                    throw new NotFoundException(nameof(MaritalStatus), request.Id);
                }

                maritalStatus.Name = request.Name;
               

                await _unitOfWork.SaveAsync(cancellationToken);

                return maritalStatus.Id;
            }
        }
}
