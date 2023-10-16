using Application.Clients.Queries;
using Application.Clients.Queries.GetClients;
using Application.Common.Interfaces.CVS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.CVS.Enums;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace Application.DriversLicences.Queries
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
        public async Task<IEnumerable<DriversLicenceDto>> Handle(GetDriversLicenceQuery request, CancellationToken cancellationToken)
        {
            return (await _unitOfWork.DriversLicenceRepository.GetAsync())
               .AsQueryable()
               .ProjectTo<DriversLicenceDto>(_mapper.ConfigurationProvider);
        }
    }
}
