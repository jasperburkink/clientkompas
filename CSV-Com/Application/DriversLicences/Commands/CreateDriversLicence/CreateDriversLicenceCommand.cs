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

    public class CreateDriversLicenceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateDriversLicenceCommand, DriversLicenceDto>
    {
        public async Task<DriversLicenceDto> Handle(CreateDriversLicenceCommand request, CancellationToken cancellationToken)
        {
            var driversLicence = new Domain.CVS.Domain.DriversLicence
            {
                Category = request.Category,
                Description = request.Description
            };

            driversLicence.AddDomainEvent(new DriversLicenceCreatedEvent(driversLicence));

            await unitOfWork.DriversLicenceRepository.InsertAsync(driversLicence, cancellationToken);

            await unitOfWork.SaveAsync(cancellationToken);

            return mapper.Map<DriversLicenceDto>(driversLicence);
        }
    }
}
