using Application.Common.Interfaces.CVS;
using Application.DriversLicences.Queries;
using Application.Extensions;
using AutoMapper;
using MediatR;

namespace Application.DriversLicences.Commands.UpdateDriversLicence
{
    public record UpdateDriversLicenceCommand : IRequest<DriversLicenceDto>
    {
        public int Id { get; init; }

        public string Category { get; set; }

        public string Description { get; set; }
    }

    public class UpdateDriversLicenceCommandHandler : IRequestHandler<UpdateDriversLicenceCommand, DriversLicenceDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDriversLicenceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DriversLicenceDto> Handle(UpdateDriversLicenceCommand request, CancellationToken cancellationToken)
        {

            var driversLicence = await _unitOfWork.DriversLicenceRepository.GetByIDAsync(request.Id, cancellationToken);

            driversLicence.AssertNotNull();

            driversLicence.Category = request.Category;

            driversLicence.Description = request.Description;

            await _unitOfWork.SaveAsync(cancellationToken);

            return _mapper.Map<DriversLicenceDto>(driversLicence);
        }
    }
}
