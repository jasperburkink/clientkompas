using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.Diagnoses.Queries.GetDiagnosis
{
    public class DiagnosisDto : IMapFrom<Diagnosis>
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
