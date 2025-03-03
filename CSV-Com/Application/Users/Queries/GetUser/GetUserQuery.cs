using Application.Common.Interfaces.CVS;
using Application.Common.Models;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Users.Queries.GetUser
{
    [Authorize(Policy = Policies.UserRead)]
    public record GetUserQuery : IRequest<Result<GetUserQueryDto>>
    {
        public int UserId { get; set; }
    }

    public class GetUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetUserQuery, Result<GetUserQueryDto>>
    {
        public async Task<Result<GetUserQueryDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByIDAsync(request.UserId, "CreatedByUser", cancellationToken);

            if (user == null)
            {
                return Result<GetUserQueryDto>.Failure("User not found!");
            }

            var userDto = mapper.Map<GetUserQueryDto>(user);

            return Result.Success(userDto);
        }
    }
}
