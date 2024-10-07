using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.Events;

namespace Application.CoachingPrograms.Commands.CreateCoachingProgram
{
    [Authorize(Policy = Policies.CoachingProgramManagement)]
    public record CreateCoachingProgramCommand : IRequest<CreateCoachingProgramCommandDto>
    {
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

    public class CreateCoachingProgramCommandHandler : IRequestHandler<CreateCoachingProgramCommand, CreateCoachingProgramCommandDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCoachingProgramCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateCoachingProgramCommandDto> Handle(CreateCoachingProgramCommand request, CancellationToken cancellationToken)
        {
            var coachingProgram = new CoachingProgram
            {
                ClientId = request.ClientId.Value,
                Title = request.Title,
                OrderNumber = request.OrderNumber,
                OrganizationId = request.OrganizationId,
                BeginDate = request.BeginDate.Value,
                EndDate = request.EndDate.Value,
                CoachingProgramType = request.CoachingProgramType.Value,
                HourlyRate = request.HourlyRate.Value,
                BudgetAmmount = request.BudgetAmmount
            };

            await _unitOfWork.CoachingProgramRepository.InsertAsync(coachingProgram, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            coachingProgram = await _unitOfWork.CoachingProgramRepository.GetByIDAsync(coachingProgram.Id, includeProperties: "Client,Organization", cancellationToken);

            coachingProgram.AddDomainEvent(new CoachingProgramCreatedEvent(coachingProgram));

            return _mapper.Map<CreateCoachingProgramCommandDto>(coachingProgram);
        }
    }
}
