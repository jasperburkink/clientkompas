using Application.Common.Interfaces.CVS;
using AutoMapper;
using Domain.CVS.Enums;
using MediatR;

namespace Application.DriversLicence.Queries
{
    public record GetDriversLicenceQuery : IRequest<IEnumerable<DriversLicenceDto>> { }

    public class GetDriversLicenceQueryHandler : IRequestHandler<GetDriversLicenceQuery, IEnumerable<DriversLicenceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDriversLicenceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all driverslicences
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DriversLicenceDto>> Handle(GetDriversLicenceQuery request, CancellationToken cancellationToken)
        {
            var driversLicences = new List<DriversLicenceDto>();

            foreach (var enumValue in Enum.GetValues(typeof(DriversLicenceEnum)))
            {
                var driversLicence = _mapper.Map<DriversLicenceDto>(enumValue);
                driversLicences.Add(driversLicence);
            }

            return driversLicences;
        }
    }
}
