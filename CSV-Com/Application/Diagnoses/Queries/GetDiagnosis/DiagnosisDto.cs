using Application.Diagnoses.Queries;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.Diagnoses.Queries.GetDiagnosis
{
    public class DiagnosisDto : IMapFrom<Diagnosis>
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
