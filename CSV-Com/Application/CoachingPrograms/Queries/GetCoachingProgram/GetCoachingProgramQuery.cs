using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using AutoMapper;
using MediatR;

namespace Application.CoachingPrograms.Queries.GetCoachingProgram
{
    public record GetCoachingProgramQuery : IRequest<GetCoachingProgramDto>
    {
        public int Id { get; init; }
    }

    public class GetCoachingProgramQueryHandler : IRequestHandler<GetCoachingProgramQuery, GetCoachingProgramDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCoachingProgramQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetCoachingProgramDto> Handle(GetCoachingProgramQuery request, CancellationToken cancellationToken)
        {
            // TODO: Find a better solution for including properties.
            var coachingProgram = await _unitOfWork.CoachingProgramRepository.GetByIDAsync(request.Id, cancellationToken: cancellationToken, includeProperties: "Organization,Client");

            return coachingProgram == null
                ? throw new NotFoundException($"CoachingProgram with id '{request.Id}' could not be found.")
                : _mapper.Map<GetCoachingProgramDto>(coachingProgram);
        }
    }

}
