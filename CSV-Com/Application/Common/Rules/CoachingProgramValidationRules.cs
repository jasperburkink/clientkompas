using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Constants;
using Domain.CVS.Enums;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class CoachingProgramValidationRules
    {
        public static IRuleBuilderOptions<T, int> ValidateCoachingProgramExists<T>(this IRuleBuilder<T, int> ruleBuilder, IUnitOfWork unitOfWork,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .MustAsync(async (id, cancellationToken) =>
                {
                    return await unitOfWork.CoachingProgramRepository.ExistsAsync(c => c.Id == id, cancellationToken);
                }).WithMessage(id => resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "CoachingProgramDoesNotExists", id));
        }

        public static IRuleBuilderOptions<T, int> ValidateCoachingProgramClient<T>(this IRuleBuilder<T, int> ruleBuilder, IUnitOfWork unitOfWork,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "ClientRequired"))
                .MustAsync(async (id, cancellationToken) =>
                {
                    return await unitOfWork.ClientRepository.ExistsAsync(c => c.Id == id, cancellationToken);
                }).WithMessage(id => resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "ClientDoesNotExists", id));
        }

        public static IRuleBuilderOptions<T, string> ValidateCoachingProgramTitle<T>(this IRuleBuilder<T, string> ruleBuilder, IUnitOfWork unitOfWork,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "TitleRequired"))
                .MaximumLength(CoachingProgramConstants.TITLE_MAXLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "TitleMaxLength", CoachingProgramConstants.TITLE_MAXLENGTH));
        }

        public static IRuleBuilderOptions<T, string> ValidateCoachingProgramOrderNumber<T>(this IRuleBuilder<T, string> ruleBuilder, IUnitOfWork unitOfWork,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .MaximumLength(CoachingProgramConstants.ORDERNUMBER_MAXLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "OrderNumberMaxLength", CoachingProgramConstants.ORDERNUMBER_MAXLENGTH));
        }

        public static IRuleBuilderOptions<T, int> ValidateCoachingProgramOrganization<T>(this IRuleBuilder<T, int> ruleBuilder, IUnitOfWork unitOfWork,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .MustAsync(async (id, cancellationToken) =>
                {
                    return await unitOfWork.OrganizationRepository.ExistsAsync(o => o.Id == id, cancellationToken);
                }).WithMessage(id => resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "OrganizationDoesNotExists", id));
        }

        public static IRuleBuilderOptions<T, CoachingProgramType> ValidateCoachingProgramCoachingProgramType<T>(this IRuleBuilder<T, CoachingProgramType> ruleBuilder, IUnitOfWork unitOfWork,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .IsInEnum()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "CoachingProgramTypeInValid"));
        }

        public static IRuleBuilderOptions<T, DateOnly> ValidateCoachingProgramFromDate<T>(
            this IRuleBuilder<T, DateOnly> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider,
            Func<T, DateOnly> toDateSelector)
        {
            return ruleBuilder
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage(resourceMessageProvider.GetMessage(typeof(WorkingContractValidationRules), "FromDateInFuture"))
                .Must((model, fromDate) => fromDate < toDateSelector(model))
                .WithMessage(resourceMessageProvider.GetMessage(typeof(WorkingContractValidationRules), "FromDateAfterUntilDate"));
        }

        public static IRuleBuilderOptions<T, DateOnly> ValidateCoachingProgramToDate<T>(
            this IRuleBuilder<T, DateOnly> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider,
            Func<T, DateOnly> fromDateSelector)
        {
            return ruleBuilder
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage(resourceMessageProvider.GetMessage(typeof(WorkingContractValidationRules), "ToDateInFuture"))
                .Must((model, toDate) => toDate > fromDateSelector(model))
                .WithMessage(resourceMessageProvider.GetMessage(typeof(WorkingContractValidationRules), "UntilDateBeforeFromDate"));
        }

        public static IRuleBuilderOptions<T, decimal> ValidateCoachingProgramBudgetAmmount<T>(this IRuleBuilder<T, decimal> ruleBuilder, IUnitOfWork unitOfWork,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(0)
                .WithMessage(id => resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "BudgetAmmountPositive", id));
        }

        public static IRuleBuilderOptions<T, decimal> ValidateCoachingProgramHourlyRate<T>(this IRuleBuilder<T, decimal> ruleBuilder, IUnitOfWork unitOfWork,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage()
                .GreaterThanOrEqualTo(0)
                .WithMessage(id => resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "BudgetAmmountPositive", id));
        }
    }
}
