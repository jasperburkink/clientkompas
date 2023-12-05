using Application.Common.Interfaces.CVS;
using Application.MaritalStatuses.Queries.GetMaritalStatus;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Events;
using MediatR;

namespace Application.MaritalStatuses.Commands.CreateMaritalStatus
{
    public record CreateMaritalStatusCommand : IRequest<MaritalStatusDto>
    {
        public string Name { get; init; }
    }

    public class CreateMaritalStatusCommandHandler : IRequestHandler<CreateMaritalStatusCommand, MaritalStatusDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateMaritalStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MaritalStatusDto> Handle(CreateMaritalStatusCommand request, CancellationToken cancellationToken)
        {
            var maritalStatus = new MaritalStatus
            {
                Name = request.Name
            };

            maritalStatus.AddDomainEvent(new MaritalStatusCreatedEvent(maritalStatus));

            await _unitOfWork.MaritalStatusRepository.InsertAsync(maritalStatus, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return _mapper.Map<MaritalStatusDto>(maritalStatus);
        }
    }
}
