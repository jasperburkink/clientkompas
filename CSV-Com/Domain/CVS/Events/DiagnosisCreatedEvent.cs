using Domain.Common;
using Domain.CVS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Events
{
    public class DiagnosisCreatedEvent: BaseEvent
    {
        public DiagnosisCreatedEvent(Diagnosis diagnosis)
        {
            Diagnosis = diagnosis;
        }

        public Diagnosis Diagnosis { get; }
    }

}
