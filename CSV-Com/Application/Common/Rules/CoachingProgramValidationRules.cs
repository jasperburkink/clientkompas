using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class CoachingProgramValidationRules
    {
        public static IRuleBuilderOptions<T, int> ValidateCoachingProgramExists<T>(this IRuleBuilder<T, int> ruleBuilder, IUnitOfWork unitOfWork)
        {
            return ruleBuilder
                .MustAsync(async (id, cancellationToken) =>
                {
                    return await unitOfWork.ClientRepository.ExistsAsync(c => c.Id == id, cancellationToken);
                }).WithMessage(id => $"Cliënt met {nameof(Client.Id)}:'{id}' bestaat niet.");
        }

        public static IRuleBuilderOptions<T, int> ValidateCoachingProgramId<T>(this IRuleBuilder<T, int> ruleBuilder, IUnitOfWork unitOfWork)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage();
        }
    }
}
