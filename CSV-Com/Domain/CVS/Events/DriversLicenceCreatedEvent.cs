using Domain.Common;
using Domain.CVS.Domain;

namespace Domain.CVS.Events
{
    public class DriversLicenceCreatedEvent(DriversLicence driversLicence) : BaseEvent
    {
        public DriversLicence DriversLicence { get; } = driversLicence;
    }


}
