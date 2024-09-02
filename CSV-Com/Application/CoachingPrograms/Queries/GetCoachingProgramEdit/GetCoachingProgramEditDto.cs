using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.CoachingPrograms.Queries.GetCoachingProgramEdit
{
    public class GetCoachingProgramEditDto : IMapFrom<CoachingProgram>
    {
        public required int Id { get; set; }

        public required int ClientId { get; set; }

        public required string Title { get; set; }

        public required string OrderNumber { get; set; }

        public int? OrganizationId { get; set; }

        public required int CoachingProgramType { get; set; }

        public required DateOnly BeginDate { get; set; }

        public required DateOnly EndDate { get; set; }

        public decimal? BudgetAmmount { get; set; }

        public required decimal HourlyRate { get; set; }

        public decimal? RemainingHours { get; set; }
    }
}
