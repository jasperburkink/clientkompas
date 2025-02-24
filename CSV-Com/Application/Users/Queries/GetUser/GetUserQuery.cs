using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Users.Queries.GetUser
{
    [Authorize(Policy = Policies.UserRead)]
    public record GetUserQuery : IRequest<GetUserQueryDto>
    {
        public int UserId { get; set; }
    }

    public class GetUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetUserQuery, GetUserQueryDto>
    {
        public async Task<GetUserQueryDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByIDAsync(request.UserId, cancellationToken: cancellationToken);

            return mapper.Map<GetUserQueryDto>(user);
        }
    }
}
