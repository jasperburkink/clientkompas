using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.Organizations.Queries.SearchOrganizations
{
    public class SearchOrganizationDto : IMapFrom<Organization>
    {
        public int Id { get; set; }

        public string OrganizationName { get; set; }

        public string PhoneNumber { get; set; }

        public string Website { get; set; }

        public string EmailAddress { get; set; }

        public string KVKNumber { get; set; }

        public string BTWNumber { get; set; }

        public string IBANNumber { get; set; }

        public string BIC { get; set; }
    }
}
