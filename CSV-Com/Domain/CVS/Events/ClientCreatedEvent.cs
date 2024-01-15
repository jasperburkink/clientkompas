using Domain.Common;
using Domain.CVS.Domain;


namespace Domain.CVS.Events
{
    public class ClientCreatedEvent : BaseEvent
    {
        public ClientCreatedEvent(Client client)
        {
            Client = client;
        }

        public Client Client { get; }
    }

}
