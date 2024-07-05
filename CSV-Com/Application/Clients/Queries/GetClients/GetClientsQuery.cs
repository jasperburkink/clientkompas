using Application.Clients.Queries.GetClient;
using Application.Common.Interfaces.CVS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Clients.Queries.GetClients
{
    //[Authorize]
    public record GetClientsQuery : IRequest<IEnumerable<GetClientDto>>;

    public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, IEnumerable<GetClientDto>>
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
        public async Task<IEnumerable<GetClientDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            // TODO: Find a better solution for including properties.
            return (await _unitOfWork.ClientRepository.GetAsync(includeProperties: "MaritalStatus,BenefitForms,DriversLicences,Diagnoses,EmergencyPeople,WorkingContracts.Organization"))
                .AsQueryable()
                .Where(c => c.DeactivationDateTime == null)
                .ProjectTo<GetClientDto>(_mapper.ConfigurationProvider)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName);
        }
    }
}
