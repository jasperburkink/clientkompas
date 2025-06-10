using Domain.Common;
using Domain.CVS.Domain;

namespace Domain.CVS.Events
{
    public class DiagnosisCreatedEvent(Diagnosis diagnosis) : BaseEvent
    {
        public Diagnosis Diagnosis { get; } = diagnosis;
    }

}
