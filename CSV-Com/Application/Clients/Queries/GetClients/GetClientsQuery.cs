using Application.Clients.Queries.GetClient;
using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Clients.Queries.GetClients
{
    [Authorize(Policy = Policies.ClientRead)]
    public record GetClientsQuery : IRequest<IEnumerable<GetClientDto>>;

    public class GetClientsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetClientsQuery, IEnumerable<GetClientDto>>
    {
        /// <summary>
        /// Get all clients
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<GetClientDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            // TODO: Find a better solution for including properties.
            return (await unitOfWork.ClientRepository.GetAsync(includeProperties: "MaritalStatus,BenefitForms,DriversLicences,Diagnoses,EmergencyPeople,WorkingContracts.Organization"))
                .AsQueryable()
                .Where(c => c.DeactivationDateTime == null)
                .ProjectTo<GetClientDto>(mapper.ConfigurationProvider)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName);
        }
    }
}
