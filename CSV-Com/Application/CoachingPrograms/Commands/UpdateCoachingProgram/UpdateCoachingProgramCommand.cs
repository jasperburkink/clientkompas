using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.Events;
using MediatR;

namespace Application.CoachingPrograms.Commands.UpdateCoachingProgram
{
    public record UpdateCoachingProgramCommand : IRequest<UpdateCoachingProgramCommandDto>
    {
        public required int Id { get; set; }

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

    public class UpdateCoachingProgramCommandHandler : IRequestHandler<UpdateCoachingProgramCommand, UpdateCoachingProgramCommandDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCoachingProgramCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateCoachingProgramCommandDto> Handle(UpdateCoachingProgramCommand request, CancellationToken cancellationToken)
        {
            var coachingProgram = await _unitOfWork.CoachingProgramRepository.GetByIDAsync(request.Id, includeProperties: "Client,Organization", cancellationToken)
                ?? throw new NotFoundException(nameof(Client), request.Id);

            coachingProgram.ClientId = request.ClientId;
            coachingProgram.Title = request.Title;
            coachingProgram.OrderNumber = request.OrderNumber;
            coachingProgram.OrganizationId = request.OrganizationId;
            coachingProgram.BeginDate = request.BeginDate;
            coachingProgram.EndDate = request.EndDate;
            coachingProgram.CoachingProgramType = request.CoachingProgramType;
            coachingProgram.HourlyRate = request.HourlyRate;
            coachingProgram.BudgetAmmount = request.BudgetAmmount;

            await _unitOfWork.CoachingProgramRepository.InsertAsync(coachingProgram, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            coachingProgram.AddDomainEvent(new CoachingProgramUpdatedEvent(coachingProgram));

            return _mapper.Map<UpdateCoachingProgramCommandDto>(coachingProgram);
        }
    }

}
