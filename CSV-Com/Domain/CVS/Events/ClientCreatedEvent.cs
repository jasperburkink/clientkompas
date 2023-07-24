using Domain.CVS.Common;
using Domain.CVS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Events
{
    public class ClientCreatedEvent: BaseEvent
    {
        public ClientCreatedEvent(Client client)
        {
            Client = client;
        }

        public Client Client { get; }
    }

}
