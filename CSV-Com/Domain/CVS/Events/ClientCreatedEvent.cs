using Domain.Common;
using Domain.CVS.Domain;


namespace Domain.CVS.Events
{
    public class ClientCreatedEvent(Client client) : BaseEvent
    {
        public Client Client { get; } = client;
    }

}
