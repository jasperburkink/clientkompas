using Application.Common.Interfaces;
using Application.Common.Validators;
using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class WorkingContractRules
    {

        public static IRuleBuilderOptions<T, int> ValidateWorkingContractOrganizationId<T>(this IRuleBuilder<T, int> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage(resourceMessageProvider.GetMessage<WorkingContractValidator>("OrganizationRequired"));
        }

        public static IRuleBuilderOptions<T, ContractType> ValidateWorkingContractContractType<T>(this IRuleBuilder<T, ContractType> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .IsInEnum()
                .WithMessage(resourceMessageProvider.GetMessage<WorkingContractValidator>("ContractInvalidValue"));
        }

        public static IRuleBuilderOptions<T, string> ValidateWorkingContractFunction<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage($"{nameof(WorkingContract.Function)} is verplicht.")
                .MaximumLength(WorkingContractConstants.FUNCTION_MAXLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage<WorkingContractValidator>("FunctionMaxCharacters", WorkingContractConstants.FUNCTION_MAXLENGTH));
        }

        public static IRuleBuilderOptions<T, DateOnly> ValidateWorkingContractFromDate<T>(
            this IRuleBuilder<T, DateOnly> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider,
            Func<T, DateOnly> toDateSelector)
        {
            return ruleBuilder
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage(resourceMessageProvider.GetMessage<WorkingContractValidator>("FromDateInFuture"))
                .Must((model, fromDate) => fromDate < toDateSelector(model))
                .WithMessage(resourceMessageProvider.GetMessage<WorkingContractValidator>("FromDateAfterUntilDate"));
        }

        public static IRuleBuilderOptions<T, DateOnly> ValidateWorkingContractToDate<T>(
            this IRuleBuilder<T, DateOnly> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider,
            Func<T, DateOnly> fromDateSelector)
        {
            return ruleBuilder
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage(resourceMessageProvider.GetMessage<WorkingContractValidator>("ToDateInFuture"))
                .Must((model, toDate) => toDate > fromDateSelector(model))
                .WithMessage(resourceMessageProvider.GetMessage<WorkingContractValidator>("UntilDateBeforeFromDate"));
        }
    }
}
