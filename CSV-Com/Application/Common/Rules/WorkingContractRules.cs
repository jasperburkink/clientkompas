using Domain.CVS.Domain;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class WorkingContractRules
    {
        //public static IRuleBuilderOptions<T, string> ValidateWorkingContractOrganization<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .NotEmpty().WithMessage($"{nameof(WorkingContract.CompanyName)} is verplicht.");
        //}

        public static IRuleBuilderOptions<T, int> ValidateWorkingContractContractType<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(WorkingContract.ContractType)} is verplicht.");
        }

        public static IRuleBuilderOptions<T, string> ValidateWorkingContractFunction<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(WorkingContract.Function)} is verplicht.");
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
