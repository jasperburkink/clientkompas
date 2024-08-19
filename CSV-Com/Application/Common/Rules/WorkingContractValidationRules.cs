using Application.Common.Interfaces;
using Domain.CVS.Constants;
using Domain.CVS.Enums;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class WorkingContractValidationRules
    {

        public static IRuleBuilderOptions<T, int> ValidateWorkingContractOrganizationId<T>(this IRuleBuilder<T, int> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(WorkingContractValidationRules), "OrganizationRequired"));
        }

        public static IRuleBuilderOptions<T, ContractType> ValidateWorkingContractContractType<T>(this IRuleBuilder<T, ContractType> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .IsInEnum()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(WorkingContractValidationRules), "ContractInvalidValue"));
        }

        public static IRuleBuilderOptions<T, string> ValidateWorkingContractFunction<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(WorkingContractValidationRules), "FunctionRequired"))
                .MaximumLength(WorkingContractConstants.FUNCTION_MAXLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(WorkingContractValidationRules), "FunctionMaxCharacters", WorkingContractConstants.FUNCTION_MAXLENGTH));
        }

        public static IRuleBuilderOptions<T, DateOnly> ValidateWorkingContractFromDate<T>(
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

        public static IRuleBuilderOptions<T, DateOnly> ValidateWorkingContractToDate<T>(
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
    }
}
