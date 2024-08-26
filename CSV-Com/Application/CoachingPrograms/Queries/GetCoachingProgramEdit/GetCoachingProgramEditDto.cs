using AutoMapper;
using Domain.CVS.Domain;

namespace Application.CoachingPrograms.Queries.GetCoachingProgramEdit
{
    // TODO: prepare for edit coachingprogram
    public class GetCoachingProgramEditDto
    {
        public required int Id { get; set; }

        public required string Title { get; set; }

        public required string OrderNumber { get; set; }

        public int? OrganizationId { get; set; }

        public required int CoachingProgramType { get; set; }

        public required DateOnly BeginDate { get; set; }

        public required DateOnly EndDate { get; set; }

        public decimal? BudgetAmmount { get; set; }

        public required decimal HourlyRate { get; set; }

        public decimal? RemainingHours { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CoachingProgram, GetCoachingProgramEditDto>()
                .ForMember(cpDto => cpDto.OrganizationId, m => m.MapFrom(cp => cp.Organization != null ? cp.Organization.Id : 0));
        }
    }
}
