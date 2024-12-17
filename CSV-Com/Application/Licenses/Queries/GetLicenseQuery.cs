using System.ComponentModel;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Licenses.Dtos;
using AutoMapper;
using MediatR;

namespace Application.Licenses.Queries
{
    public record GetLicenseQuery(int Id) : IRequest<LicenseDto>;

    public class GetLicenseQueryHandler : IRequestHandler<GetLicenseQuery, LicenseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetLicenseQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LicenseDto> Handle(GetLicenseQuery request, CancellationToken cancellationToken)
        {
            var license = await _unitOfWork.LicenseRepository
                .GetByIDAsync(request.Id, includeProperties: "Organization,LicenseHolder", cancellationToken: cancellationToken) ?? throw new NotFoundException(nameof(License), request.Id);
            return _mapper.Map<LicenseDto>(license);
        }
    }
}
