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

        public static IRuleBuilderOptions<T, int> ValidateWorkingContractOrganization<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0).WithMessage($"{nameof(WorkingContract.Organization)} is verplicht.");
        }

        public static IRuleBuilderOptions<T, ContractType> ValidateWorkingContractContractType<T>(this IRuleBuilder<T, ContractType> ruleBuilder)
        {
            return ruleBuilder
                .IsInEnum().WithMessage($"{nameof(WorkingContract.ContractType)} heeft een ongeldige waarde.");
        }

        public static IRuleBuilderOptions<T, string> ValidateWorkingContractFunction<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(WorkingContract.Function)} is verplicht.")
                .MaximumLength(WorkingContractConstants.FUNCTION_MAXLENGTH).WithMessage($"{nameof(WorkingContract.Function)} mag niet langer zijn dan {WorkingContractConstants.FUNCTION_MAXLENGTH} karakters.");
        }

        public static IRuleBuilderOptions<T, DateOnly> ValidateWorkingContractFromDate<T>(this IRuleBuilder<T, DateOnly> ruleBuilder, Func<T, DateOnly> toDateSelector)
        {
            return ruleBuilder
                .Must((model, fromDate) => fromDate < toDateSelector(model)).WithMessage($"De begindatum kan niet na de einddatum zijn.")
                .GreaterThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage($"{nameof(WorkingContract.)}");
        }

        public static IRuleBuilderOptions<T, DateOnly> ValidateWorkingContractToDate<T>(this IRuleBuilder<T, DateOnly> ruleBuilder, Func<T, DateOnly> fromDateSelector)
        {
            return ruleBuilder
                .Must((model, toDate) => toDate > fromDateSelector(model)).WithMessage($"De einddatum kan niet voor de begindatum zijn.");
        }
    }
}
