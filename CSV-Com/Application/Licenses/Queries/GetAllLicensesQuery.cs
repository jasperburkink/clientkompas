using Application.Common.Interfaces.CVS;
using Application.Licenses.Dtos;
using AutoMapper;
using MediatR;

namespace Application.Licenses.Queries
{
    public record GetAllLicensesQuery : IRequest<List<LicenseDto>>;

    public class GetAllLicensesQueryHandler : IRequestHandler<GetAllLicensesQuery, List<LicenseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllLicensesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<LicenseDto>> Handle(GetAllLicensesQuery request, CancellationToken cancellationToken)
        {
            var licenses = await _unitOfWork.LicenseRepository
                .GetAsync(includeProperties: "Organization,LicenseHolder", cancellationToken: cancellationToken);

            return _mapper.Map<List<LicenseDto>>(licenses);
        }
    }
}
