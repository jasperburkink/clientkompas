using Domain.Common;
using Domain.CVS.Domain;

namespace Domain.CVS.Events
{
    public class CoachingProgramCreatedEvent : BaseEvent
    {
        public CoachingProgramCreatedEvent(CoachingProgram coachingProgram)
        {
            CoachingProgram = coachingProgram;
        }

        public CoachingProgram CoachingProgram { get; }
    }
}
