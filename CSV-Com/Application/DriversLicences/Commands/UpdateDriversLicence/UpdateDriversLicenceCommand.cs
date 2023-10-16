using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.DriversLicences.Commands.UpdateDriversLicence
{
    public record UpdateDriversLicenceCommand : IRequest<DriversLicence>
    {
        public int Id { get; init; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
    public class UpdateDriversLicenceCommandHandler : IRequestHandler<UpdateDriversLicenceCommand, DriversLicence>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateDriversLicenceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DriversLicence> Handle(UpdateDriversLicenceCommand request, CancellationToken cancellationToken)
        {
            var driversLicence = await _unitOfWork.DriversLicenceRepository.GetByIDAsync(request.Id, cancellationToken);
            if (driversLicence == null)
            {
                throw new NotFoundException(nameof(DriversLicences), request.Id);
            }
            driversLicence.Category = request.Category;
            driversLicence.Description = request.Description;
            await _unitOfWork.SaveAsync(cancellationToken);
            return driversLicence;
        }
    } 
}
