using Domain.Common;
using Domain.CVS.Domain;

namespace Domain.CVS.Events
{
    public class OrganizationCreatedEvent(Organization organization) : BaseEvent
    {
        public Organization Organization { get; } = organization;
    }
}
