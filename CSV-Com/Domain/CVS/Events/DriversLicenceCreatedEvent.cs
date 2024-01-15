using Domain.Common;
using Domain.CVS.Domain;

namespace Domain.CVS.Events
{
    public class DriversLicenceCreatedEvent : BaseEvent
    {
        public DriversLicenceCreatedEvent(DriversLicence driversLicence)
        {
            DriversLicence = driversLicence;
        }

        public DriversLicence DriversLicence { get; }
    }


}
