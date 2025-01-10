using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.CoachingPrograms.Queries.GetCoachingProgramEdit
{
    [Authorize(Policy = Policies.CoachingProgramManagement)]
    public record GetCoachingProgramEditQuery : IRequest<GetCoachingProgramEditDto>
    {
        public int Id { get; init; }
    }

    public class GetCoachingProgramEditQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetCoachingProgramEditQuery, GetCoachingProgramEditDto>
    {
        public async Task<GetCoachingProgramEditDto> Handle(GetCoachingProgramEditQuery request, CancellationToken cancellationToken)
        {
            var coachingProgram = await unitOfWork.CoachingProgramRepository.GetByIDAsync(request.Id, cancellationToken: cancellationToken);

            return coachingProgram == null
                ? throw new NotFoundException($"Coaching program with id '{request.Id}' does not exist.")
                : mapper.Map<GetCoachingProgramEditDto>(coachingProgram);
        }
    }
}
