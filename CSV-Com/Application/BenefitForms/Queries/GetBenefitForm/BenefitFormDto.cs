using Application.BenefitForms.Queries;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BenefitForms.Queries.GetBenefitForm
{
    public class BenefitFormDto : IMapFrom<BenefitForm>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}