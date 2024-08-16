using Application.Common.Interfaces.CVS;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.Events;
using MediatR;

namespace Application.CoachingPrograms.Commands.CreateCoachingProgram
{
    public record CreateCoachingProgramCommand : IRequest<CreateCoachingProgramCommandDto>
    {
        public required int ClientId { get; set; }

        public required string Title { get; set; }

        public string? OrderNumber { get; set; }

        public int? OrganizationId { get; set; }

        public required CoachingProgramType CoachingProgramType { get; set; }

        public required DateOnly BeginDate { get; set; }

        public required DateOnly EndDate { get; set; }

        public decimal? BudgetAmmount { get; set; }

        public required decimal HourlyRate { get; set; }
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
                ClientId = request.ClientId,
                Title = request.Title,
                OrderNumber = request.OrderNumber,
                OrganizationId = request.OrganizationId,
                BeginDate = request.BeginDate,
                EndDate = request.EndDate,
                CoachingProgramType = request.CoachingProgramType,
                HourlyRate = request.HourlyRate,
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
