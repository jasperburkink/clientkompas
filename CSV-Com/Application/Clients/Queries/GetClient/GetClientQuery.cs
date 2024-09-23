using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Clients.Queries.GetClient
{
    [Authorize(Policy = Policies.ClientManagement)]
    public record GetClientQuery : IRequest<GetClientDto>
    {
        public int ClientId { get; init; }
    }

    public class GetClientQueryHandler : IRequestHandler<GetClientQuery, GetClientDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
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
        public async Task<GetClientDto> Handle(GetClientQuery request, CancellationToken cancellationToken)
        {
            // TODO: Find a better solution for including properties.
            var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.ClientId, cancellationToken: cancellationToken, includeProperties: "DriversLicences,BenefitForms,Diagnoses,EmergencyPeople,WorkingContracts.Organization,MaritalStatus");

            return client == null
                ? throw new NotFoundException($"Client with id '{request.ClientId}' could not be found.")
                : _mapper.Map<GetClientDto>(client);
        }
    }
}
