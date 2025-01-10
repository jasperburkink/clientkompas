using Domain.Common;
using Domain.CVS.Domain;

namespace Domain.CVS.Events
{
    public class CoachingProgramUpdatedEvent(CoachingProgram coachingProgram) : BaseEvent
    {
        public CoachingProgram CoachingProgram { get; } = coachingProgram;
    }
}
