using System.Globalization;
using Application.CoachingPrograms.Queries.GetCoachingProgram;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Resources;
using AutoMapper;
using Domain.CVS.Enums;

namespace Application.CoachingPrograms.Queries.GetCoachingProgramTypes
{
    public class GetCoachingProgramTypesDto : IMapFrom<CoachingProgramType>
    {
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public GetCoachingProgramTypesDto()
        {
            _resourceMessageProvider = new ResourceMessageProvider(CultureInfo.CurrentUICulture);
        }

        public required int Id { get; set; }

        public required string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CoachingProgramType, GetCoachingProgramTypesDto>()
                .ForMember(cpTypeDto => cpTypeDto.Id, m => m.MapFrom(type => type))
                .ForMember(cpTypeDto => cpTypeDto.Name, m => m.MapFrom(type => _resourceMessageProvider.GetMessage(typeof(GetCoachingProgramDto), Enum.GetName(typeof(CoachingProgramType), type))));
        }
    }
}
