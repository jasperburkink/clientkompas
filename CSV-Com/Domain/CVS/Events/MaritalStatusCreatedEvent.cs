using Domain.Common;
using Domain.CVS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Events
{
    public class MaritalStatusCreatedEvent : BaseEvent
    {
        public MaritalStatusCreatedEvent(MaritalStatus maritalStatus)
        {
            MaritalStatus = maritalStatus;
        }

        public MaritalStatus MaritalStatus { get; }
    }

}
