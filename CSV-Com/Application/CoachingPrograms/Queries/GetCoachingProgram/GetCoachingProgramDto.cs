using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;

namespace Application.CoachingPrograms.Queries.GetCoachingProgram
{
    public class GetCoachingProgramDto : IMapFrom<CoachingProgram>
    {
        public required int Id { get; set; }

        public required string ClientFullName { get; set; }

        public required string Title { get; set; }

        public required string OrderNumber { get; set; }

        public string? OrganizationName { get; set; }

        public required CoachingProgramType CoachingProgramType { get; set; }

        public required DateOnly BeginDate { get; set; }

        public required DateOnly EndDate { get; set; }

        public decimal? BudgetAmmount { get; set; }

        public required decimal HourlyRate { get; set; }

        public decimal? RemainingHours { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CoachingProgram, GetCoachingProgramDto>()
                .ForMember(cpDto => cpDto.ClientFullName, m => m.MapFrom(cp => cp.Client.FullName))
                .ForMember(cpDto => cpDto.OrganizationName, m => m.MapFrom(cp => cp.Organization.OrganizationName));
        }
    }
}
