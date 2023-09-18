using Domain.Common;
using Domain.CVS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
