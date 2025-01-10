using Domain.Common;
using Domain.CVS.Domain;

namespace Domain.CVS.Events
{
    public class MaritalStatusCreatedEvent(MaritalStatus maritalStatus) : BaseEvent
    {
        public MaritalStatus MaritalStatus { get; } = maritalStatus;
    }
}
