using Domain.Common;
using Domain.CVS.Domain;

namespace Domain.CVS.Events
{
    public class CoachingProgramUpdatedEvent : BaseEvent
    {
        public CoachingProgramUpdatedEvent(CoachingProgram coachingProgram)
        {
            CoachingProgram = coachingProgram;
        }

        public CoachingProgram CoachingProgram { get; }
    }
}
