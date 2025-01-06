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

    public class GetClientFullnameQueryHandler : IRequestHandler<GetClientFullnameQuery, GetClientFullnameDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientFullnameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetClientFullnameDto> Handle(GetClientFullnameQuery request, CancellationToken cancellationToken)
        {
            var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.ClientId, cancellationToken: cancellationToken);

            return client == null
                ? throw new NotFoundException($"Client with id '{request.ClientId}' could not be found.")
                : _mapper.Map<GetClientFullnameDto>(client);
        }
    }
}
