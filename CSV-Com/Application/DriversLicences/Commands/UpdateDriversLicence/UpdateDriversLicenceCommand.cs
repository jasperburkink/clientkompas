using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Application.DriversLicences.Queries;
using Application.Extensions;
using Domain.Authentication.Constants;

namespace Application.DriversLicences.Commands.UpdateDriversLicence
{
    [Authorize(Policy = Policies.DriversLicenceManagement)]
    public record UpdateDriversLicenceCommand : IRequest<DriversLicenceDto>
    {
        public int Id { get; init; }

        public string Category { get; set; }

        public string Description { get; set; }
    }

    public class UpdateDriversLicenceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateDriversLicenceCommand, DriversLicenceDto>
    {
        public async Task<DriversLicenceDto> Handle(UpdateDriversLicenceCommand request, CancellationToken cancellationToken)
        {

            var driversLicence = await unitOfWork.DriversLicenceRepository.GetByIDAsync(request.Id, cancellationToken);

            driversLicence.AssertNotNull();

            driversLicence.Category = request.Category;

            driversLicence.Description = request.Description;

            await unitOfWork.SaveAsync(cancellationToken);

            return mapper.Map<DriversLicenceDto>(driversLicence);
        }
    }
}
