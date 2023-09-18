using Application.Clients.Commands.CreateClient;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DriversLicence.Commands.CreateDriversLicence
{
    public record CreateDriversLicenceCommand : IRequest<int>
    {

        public string Category { get; set; }

        public string Omschrijving { get; set; }
        

      
    }
    public class CreateDriversLicenceCommandHandler : IRequestHandler<CreateDriversLicenceCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateDriversLicenceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateDriversLicenceCommand request, CancellationToken cancellationToken)
        {
            var driversLicence = new Domain.CVS.Domain.DriversLicence
            {
                Category = request.Category,
                Omschrijving = request.Omschrijving
        
               
            };

            driversLicence.AddDomainEvent(new DriversLicenceCreatedEvent(driversLicence));

            await _unitOfWork.DriversLicenceRepository.InsertAsync(driversLicence, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return driversLicence.Id;
        }
    }
}
