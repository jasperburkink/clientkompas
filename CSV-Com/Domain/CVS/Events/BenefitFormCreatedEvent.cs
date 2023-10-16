using Domain.Common;
using Domain.CVS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Events
{
    public class BenefitFormCreatedEvent : BaseEvent
    {
        public BenefitFormCreatedEvent(BenefitForm benefitForm)
        {
            BenefitForm = benefitForm;
        }

        public BenefitForm BenefitForm { get; }
    }

}
