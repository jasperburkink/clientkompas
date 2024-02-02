using System.Text.Json;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;

namespace Application.Diagnoses.Queries.GetDiagnosis
{
    public class DiagnosisDto : IMapFrom<Diagnosis>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Diagnosis ToDomainModel(IMapper mapper, Client client)
        {
            var domainModel = JsonSerializer.Deserialize<Diagnosis>(JsonSerializer.Serialize(this));
            return domainModel;
        }
    }
}
