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

    public class GetClientEditQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetClientEditQuery, GetClientEditDto>
    {
        public async Task<GetClientEditDto> Handle(GetClientEditQuery request, CancellationToken cancellationToken)
        {
            // TODO: Find a better solution for including properties.
            var client = await unitOfWork.ClientRepository.GetByIDAsync(request.ClientId, cancellationToken: cancellationToken, includeProperties: "DriversLicences,BenefitForms,Diagnoses,EmergencyPeople,WorkingContracts,MaritalStatus");

            return client == null
                ? throw new NotFoundException($"Client with id '{request.ClientId}' does not exist.")
                : mapper.Map<GetClientEditDto>(client);
        }
    }
}
