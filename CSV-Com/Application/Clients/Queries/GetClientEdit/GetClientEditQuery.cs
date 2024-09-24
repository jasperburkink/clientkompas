using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Clients.Queries.GetClientEdit
{
    [Authorize(Policy = Policies.ClientManagement)]
    public record GetClientEditQuery : IRequest<GetClientEditDto>
    {
        public int ClientId { get; init; }
    }

    public class GetClientEditQueryHandler : IRequestHandler<GetClientEditQuery, GetClientEditDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientEditQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetClientEditDto> Handle(GetClientEditQuery request, CancellationToken cancellationToken)
        {
            // TODO: Find a better solution for including properties.
            var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.ClientId, cancellationToken: cancellationToken, includeProperties: "DriversLicences,BenefitForms,Diagnoses,EmergencyPeople,WorkingContracts,MaritalStatus");

            return client == null
                ? throw new NotFoundException($"Client with id '{request.ClientId}' does not exist.")
                : _mapper.Map<GetClientEditDto>(client);
        }
    }
}
