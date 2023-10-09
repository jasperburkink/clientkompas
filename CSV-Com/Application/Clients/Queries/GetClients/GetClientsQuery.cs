using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Clients.Queries.GetClients
{
    //[Authorize]
    public record GetClientsQuery : IRequest<IEnumerable<ClientDto>>;

    public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, IEnumerable<ClientDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all clients
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ClientDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            // TODO: Find a better solution for including properties.
            return (await _unitOfWork.ClientRepository.GetAsync(includeProperties: "MaritalStatus,DriversLicences,Diagnoses,EmergencyPeople,WorkingContracts"))
                .AsQueryable()
                .ProjectTo<ClientDto>(_mapper.ConfigurationProvider)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName);
        }
    }
}
