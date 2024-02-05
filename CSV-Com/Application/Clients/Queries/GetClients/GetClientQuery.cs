using Application.Common.Interfaces.CVS;
using AutoMapper;
using MediatR;

namespace Application.Clients.Queries.GetClients
{
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
            var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.ClientId, cancellationToken: cancellationToken, includeProperties: "DriversLicences,Diagnoses,EmergencyPeople,WorkingContracts,MaritalStatus");
            return _mapper.Map<GetClientDto>(client);
        }
    }
}
