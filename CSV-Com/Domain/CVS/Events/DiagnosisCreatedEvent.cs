using Domain.Common;
using Domain.CVS.Domain;

namespace Domain.CVS.Events
{
    public class DiagnosisCreatedEvent : BaseEvent
    {
        public DiagnosisCreatedEvent(Diagnosis diagnosis)
        {
            Diagnosis = diagnosis;
        }

        public Diagnosis Diagnosis { get; }
    }

}
