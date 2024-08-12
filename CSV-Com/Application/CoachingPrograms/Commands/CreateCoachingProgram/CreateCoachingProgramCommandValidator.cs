using Application.Common.Interfaces.CVS;
using FluentValidation;

namespace Application.CoachingPrograms.Commands.CreateCoachingProgram
{
    public class CreateCoachingProgramCommandValidator : AbstractValidator<CreateCoachingProgramCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCoachingProgramCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(cp => cp.ClientId).ValidateClient();
        }
    }
}
