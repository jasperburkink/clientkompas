using Domain.Common;
using Domain.CVS.Enums;

namespace Domain.CVS.Domain
{
    public class CoachingProgram : BaseAuditableEntity
    {
        public required Client Client { get; set; }

        public required int ClientId { get; set; }

        public required string Title { get; set; }

        public string? OrderNumber { get; set; }

        public Organization? Organization { get; set; }

        public int? OrganizationId { get; set; }

        public required CoachingProgramType CoachingProgramType { get; set; }

        public required DateOnly BeginDate { get; set; }

        public required DateOnly EndDate { get; set; }

        public decimal? BudgetAmmount { get; set; }

        public required decimal HourlyRate { get; set; }

        public decimal RemainingHours => HourlyRate == 0 ? 0 : (BudgetAmmount ?? 0) / HourlyRate;

    }
}
