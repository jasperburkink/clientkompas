using Domain.Common;
using Domain.CVS.Domain;

namespace Domain.CVS.Events
{
    public class OrganizationCreatedEvent : BaseEvent
    {
        public OrganizationCreatedEvent(Organization organization)
        {
            Organization = organization;
        }

        public Organization Organization { get; }
    }
}
