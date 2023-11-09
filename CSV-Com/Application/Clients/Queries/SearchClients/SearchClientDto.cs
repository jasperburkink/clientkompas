using Application.Clients.Queries.GetClients;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
