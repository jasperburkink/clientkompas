using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class WorkingContractRules
    {
        public static IRuleBuilderOptions<T, int> ValidateWorkingContractOrganizationId<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0).WithMessage($"{nameof(WorkingContract.OrganizationId)} is verplicht.");
        }

        public static IRuleBuilderOptions<T, ContractType> ValidateWorkingContractContractType<T>(this IRuleBuilder<T, ContractType> ruleBuilder)
        {
            return ruleBuilder
                .IsInEnum().WithMessage($"{nameof(WorkingContract.ContractType)} heeft een ongeldige waarde.")
                .NotNull().WithMessage($"{nameof(WorkingContract.ContractType)} is verplicht.");
        }

        public static IRuleBuilderOptions<T, string> ValidateWorkingContractFunction<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(WorkingContract.Function)} is verplicht.")
                .MaximumLength(WorkingContractConstants.FUNCTION_MAXLENGTH).WithMessage($"{nameof(WorkingContract.Function)} mag niet langer zijn dan {WorkingContractConstants.FUNCTION_MAXLENGTH} karakters.");
        }

        public static IRuleBuilderOptions<T, DateOnly> ValidateWorkingContractFromDate<T>(this IRuleBuilder<T, DateOnly> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(WorkingContract.FromDate)} is verplicht.");
        }

        public static IRuleBuilderOptions<T, DateOnly> ValidateWorkingContractToDate<T>(this IRuleBuilder<T, DateOnly> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(WorkingContract.ToDate)} is verplicht.");
        }
    }
}
