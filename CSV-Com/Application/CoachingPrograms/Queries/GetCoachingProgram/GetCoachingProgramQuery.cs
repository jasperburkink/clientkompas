using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.CoachingPrograms.Queries.GetCoachingProgram
{
    [Authorize(Policy = Policies.CoachingProgramRead)]
    public record GetCoachingProgramQuery : IRequest<GetCoachingProgramDto>
    {
        public int Id { get; init; }
    }

    public class GetCoachingProgramQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetCoachingProgramQuery, GetCoachingProgramDto>
    {
        public async Task<GetCoachingProgramDto> Handle(GetCoachingProgramQuery request, CancellationToken cancellationToken)
        {
            // TODO: Find a better solution for including properties.
            var coachingProgram = await unitOfWork.CoachingProgramRepository.GetByIDAsync(request.Id, cancellationToken: cancellationToken, includeProperties: "Organization,Client");

            return coachingProgram == null
                ? throw new NotFoundException($"CoachingProgram with id '{request.Id}' could not be found.")
                : mapper.Map<GetCoachingProgramDto>(coachingProgram);
        }
    }
}
