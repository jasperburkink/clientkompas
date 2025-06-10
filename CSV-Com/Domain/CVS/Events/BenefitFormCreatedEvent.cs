using Domain.Common;
using Domain.CVS.Domain;

namespace Domain.CVS.Events
{
    public class BenefitFormCreatedEvent(BenefitForm benefitForm) : BaseEvent
    {
        public BenefitForm BenefitForm { get; } = benefitForm;
    }
}
