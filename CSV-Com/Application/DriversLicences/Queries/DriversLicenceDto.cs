using Application.Clients.Queries;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DriversLicences.Queries
{
    public class DriversLicenceDto : IMapFrom<DriversLicence>
    {

        public int Id { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }




    }
}
