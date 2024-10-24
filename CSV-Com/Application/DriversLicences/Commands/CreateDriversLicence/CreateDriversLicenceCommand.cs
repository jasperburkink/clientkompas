using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Application.DriversLicences.Queries;
using Domain.Authentication.Constants;
using Domain.CVS.Events;

namespace Application.DriversLicences.Commands.CreateDriversLicence
{
    [Authorize(Policy = Policies.DriversLicenceManagement)]
    public record CreateDriversLicenceCommand : IRequest<DriversLicenceDto>
    {
        public string Category { get; set; }

        public string Description { get; set; }
    }

    public class CreateDriversLicenceCommandHandler : IRequestHandler<CreateDriversLicenceCommand, DriversLicenceDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDriversLicenceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DriversLicenceDto> Handle(CreateDriversLicenceCommand request, CancellationToken cancellationToken)
        {
            var driversLicence = new Domain.CVS.Domain.DriversLicence
            {
                Category = request.Category,
                Description = request.Description
            };

            driversLicence.AddDomainEvent(new DriversLicenceCreatedEvent(driversLicence));

            await _unitOfWork.DriversLicenceRepository.InsertAsync(driversLicence, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return _mapper.Map<DriversLicenceDto>(driversLicence);
        }
    }
}
