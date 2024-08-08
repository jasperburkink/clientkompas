using Domain.Common;
using Domain.CVS.Enums;

namespace Domain.CVS.Domain
{
    public class CoachingProgram : BaseAuditableEntity
    {
        public required string Title { get; set; }

        public required string OrderNumber { get; set; }

        public Organization? Organization { get; set; }

        public int? OrganizationId { get; set; }

        public required CoachingProgramType CoachingProgramType { get; set; }

        public required DateOnly BeginDate { get; set; }

        public required DateOnly EndDate { get; set; }

        public required decimal BudgetAmmount { get; set; }

        public required decimal HourlyRate { get; set; }
    }
}
