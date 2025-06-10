using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.Events;

namespace Application.CoachingPrograms.Commands.UpdateCoachingProgram
{
    [Authorize(Policy = Policies.CoachingProgramManagement)]
    public record UpdateCoachingProgramCommand : IRequest<UpdateCoachingProgramCommandDto>
    {
        public required int Id { get; set; }

        public int? ClientId { get; set; }

        public string? Title { get; set; }

        public string? OrderNumber { get; set; }

        public int? OrganizationId { get; set; }

        public CoachingProgramType? CoachingProgramType { get; set; }

        public DateOnly? BeginDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public decimal? BudgetAmmount { get; set; }

        public decimal? HourlyRate { get; set; }
    }

    public class UpdateCoachingProgramCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateCoachingProgramCommand, UpdateCoachingProgramCommandDto>
    {
        public async Task<UpdateCoachingProgramCommandDto> Handle(UpdateCoachingProgramCommand request, CancellationToken cancellationToken)
        {
            var coachingProgram = await unitOfWork.CoachingProgramRepository.GetByIDAsync(request.Id, includeProperties: "Client,Organization", cancellationToken)
                ?? throw new NotFoundException(nameof(Client), request.Id);

            coachingProgram.ClientId = request.ClientId.Value;
            coachingProgram.Title = request.Title;
            coachingProgram.OrderNumber = request.OrderNumber;
            coachingProgram.OrganizationId = request.OrganizationId;
            coachingProgram.BeginDate = request.BeginDate.Value;
            coachingProgram.EndDate = request.EndDate.Value;
            coachingProgram.CoachingProgramType = request.CoachingProgramType.Value;
            coachingProgram.HourlyRate = request.HourlyRate.Value;
            coachingProgram.BudgetAmmount = request.BudgetAmmount;

            await unitOfWork.CoachingProgramRepository.UpdateAsync(coachingProgram, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);

            coachingProgram.AddDomainEvent(new CoachingProgramUpdatedEvent(coachingProgram));

            coachingProgram = await unitOfWork.CoachingProgramRepository.GetByIDAsync(coachingProgram.Id, includeProperties: "Client,Organization", cancellationToken);

            return mapper.Map<UpdateCoachingProgramCommandDto>(coachingProgram);
        }
    }

}
