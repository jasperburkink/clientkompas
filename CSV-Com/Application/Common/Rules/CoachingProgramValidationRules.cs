using Application.CoachingPrograms.Commands.CreateCoachingProgram;
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

        public static IRuleBuilderOptions<T, int?> ValidateCoachingProgramClient<T>(this IRuleBuilder<T, int?> ruleBuilder, IUnitOfWork unitOfWork,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(id => resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "ClientRequired", id))
                .MustAsync(async (id, cancellationToken) =>
                {
                    return await unitOfWork.ClientRepository.ExistsAsync(c => c.Id == id, cancellationToken);
                })
                .WithMessage(createCoachingProgramCommandT =>
                {
                    var createCoachingProgramCommand = createCoachingProgramCommandT as CreateCoachingProgramCommand;
                    var clientId = createCoachingProgramCommand?.ClientId ?? 0;
                    return resourceMessageProvider.GetMessage(
                        typeof(CoachingProgramValidationRules),
                        "ClientDoesNotExists",
                        clientId
                    );
                });
        }

        public static IRuleBuilderOptions<T, string?> ValidateCoachingProgramTitle<T>(this IRuleBuilder<T, string?> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "TitleRequired"))
                .MaximumLength(CoachingProgramConstants.TITLE_MAXLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "TitleMaxLength", CoachingProgramConstants.TITLE_MAXLENGTH));
        }

        public static IRuleBuilderOptions<T, string?> ValidateCoachingProgramOrderNumber<T>(this IRuleBuilder<T, string?> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .MaximumLength(CoachingProgramConstants.ORDERNUMBER_MAXLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "OrderNumberMaxLength", CoachingProgramConstants.ORDERNUMBER_MAXLENGTH));
        }

        public static IRuleBuilderOptions<T, int?> ValidateCoachingProgramOrganization<T>(this IRuleBuilder<T, int?> ruleBuilder, IUnitOfWork unitOfWork,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .MustAsync(async (id, cancellationToken) =>
                {
                    return id == null || await unitOfWork.OrganizationRepository.ExistsAsync(o => o.Id == id, cancellationToken);
                })
                .WithMessage(createCoachingProgramCommandT =>
                 {
                     var createCoachingProgramCommand = createCoachingProgramCommandT as CreateCoachingProgramCommand;
                     var organizationId = createCoachingProgramCommand?.OrganizationId ?? 0;
                     return resourceMessageProvider.GetMessage(
                         typeof(CoachingProgramValidationRules),
                         "OrganizationDoesNotExists",
                         organizationId
                     );
                 });
        }

        public static IRuleBuilderOptions<T, CoachingProgramType?> ValidateCoachingProgramCoachingProgramType<T>(this IRuleBuilder<T, CoachingProgramType?> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .IsInEnum()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "CoachingProgramTypeInValid"));
        }

        public static IRuleBuilderOptions<T, DateOnly?> ValidateCoachingProgramFromDate<T>(
            this IRuleBuilder<T, DateOnly?> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider,
            Func<T, DateOnly?> toDateSelector)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "FromDateRequired"))
                .Must((model, fromDate) => fromDate < toDateSelector(model))
                .WithMessage(resourceMessageProvider.GetMessage(typeof(WorkingContractValidationRules), "FromDateAfterUntilDate"));
        }

        public static IRuleBuilderOptions<T, DateOnly?> ValidateCoachingProgramToDate<T>(
            this IRuleBuilder<T, DateOnly?> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider,
            Func<T, DateOnly?> fromDateSelector)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "UntilDateRequired"))
                .Must((model, toDate) => toDate > fromDateSelector(model))
                .WithMessage(resourceMessageProvider.GetMessage(typeof(WorkingContractValidationRules), "UntilDateBeforeFromDate"));
        }

        public static IRuleBuilderOptions<T, decimal?> ValidateCoachingProgramBudgetAmmount<T>(this IRuleBuilder<T, decimal?> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(0)
                .WithMessage(id => resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "BudgetAmmountPositive", id));
        }

        public static IRuleBuilderOptions<T, decimal?> ValidateCoachingProgramHourlyRate<T>(this IRuleBuilder<T, decimal?> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "HourlyRateRequired"))
                .GreaterThanOrEqualTo(0)
                .WithMessage(id => resourceMessageProvider.GetMessage(typeof(CoachingProgramValidationRules), "HourlyRatePositive", id));
        }
    }
}
