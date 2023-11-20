using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.Clients.Queries.SearchClients
{
    public class SearchClientDto : IMapFrom<Client>
    {
        public int IdentificationNumber { get; set; }

        public string FirstName { get; set; }

        public string Initials { get; set; }

        public string PrefixLastName { get; set; }

        public string LastName { get; set; }
    }
}
