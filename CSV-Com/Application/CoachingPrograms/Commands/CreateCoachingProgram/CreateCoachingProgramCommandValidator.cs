using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using FluentValidation;

namespace Application.CoachingPrograms.Commands.CreateCoachingProgram
{
    public class CreateCoachingProgramCommandValidator : AbstractValidator<CreateCoachingProgramCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public CreateCoachingProgramCommandValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;
            RuleFor(cp => cp.ClientId).ValidateCoachingProgramClient(_unitOfWork, _resourceMessageProvider);
            RuleFor(cp => cp.Title).ValidateCoachingProgramTitle(_resourceMessageProvider);
            RuleFor(cp => cp.OrderNumber).ValidateCoachingProgramOrderNumber(_resourceMessageProvider);
            RuleFor(cp => cp.OrganizationId).ValidateCoachingProgramOrganization(_unitOfWork, _resourceMessageProvider);
            RuleFor(cp => cp.CoachingProgramType).ValidateCoachingProgramCoachingProgramType(_resourceMessageProvider);
            RuleFor(cp => cp.BeginDate).ValidateCoachingProgramFromDate(_resourceMessageProvider, cp => cp.EndDate);
            RuleFor(cp => cp.EndDate).ValidateCoachingProgramToDate(_resourceMessageProvider, cp => cp.BeginDate);
            RuleFor(cp => cp.BudgetAmmount).ValidateCoachingProgramBudgetAmmount(_resourceMessageProvider);
            RuleFor(cp => cp.HourlyRate).ValidateCoachingProgramHourlyRate(_resourceMessageProvider);
        }
    }
}
