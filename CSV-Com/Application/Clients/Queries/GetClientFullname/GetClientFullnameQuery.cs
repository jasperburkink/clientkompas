using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Clients.Queries.GetClientFullname
{
    [Authorize(Policy = Policies.ClientRead)]
    public record GetClientFullnameQuery : IRequest<GetClientFullnameDto>
    {
        public int ClientId { get; init; }
    }

    public class GetClientFullnameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetClientFullnameQuery, GetClientFullnameDto>
    {
        public async Task<GetClientFullnameDto> Handle(GetClientFullnameQuery request, CancellationToken cancellationToken)
        {
            var client = await unitOfWork.ClientRepository.GetByIDAsync(request.ClientId, cancellationToken: cancellationToken);

            return client == null
                ? throw new NotFoundException($"Client with id '{request.ClientId}' could not be found.")
                : mapper.Map<GetClientFullnameDto>(client);
        }
    }
}
