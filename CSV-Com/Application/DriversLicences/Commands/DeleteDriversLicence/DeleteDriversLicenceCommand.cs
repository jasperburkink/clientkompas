using Application.DriversLicences.Queries;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using MediatR;
using AutoMapper;
using Application.Extensions;

namespace Application.Clients.Commands.DeleteClientDriversLicences
{
    public record DeleteDriversLicenceCommand : IRequest
    {
        public int DriversLicenceId { get; init; }
    }

    public class DeleteDriversLicenceCommandHandler : IRequestHandler<DeleteDriversLicenceCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteDriversLicenceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(DeleteDriversLicenceCommand request, CancellationToken cancellationToken)
        {        

            var driversLicence = await _unitOfWork.DriversLicenceRepository.GetByIDAsync(request.DriversLicenceId, cancellationToken);

            driversLicence.AssertNotNull();

            await _unitOfWork.DriversLicenceRepository.DeleteAsync(driversLicence);

            await _unitOfWork.SaveAsync(cancellationToken);

            
        }
    }
}
