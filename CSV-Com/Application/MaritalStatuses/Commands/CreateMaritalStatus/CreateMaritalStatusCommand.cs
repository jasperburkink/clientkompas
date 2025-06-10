using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Application.MaritalStatuses.Queries.GetMaritalStatus;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Domain.CVS.Events;

namespace Application.MaritalStatuses.Commands.CreateMaritalStatus
{
    [Authorize(Policy = Policies.MaritalStatusManagement)]
    public record CreateMaritalStatusCommand : IRequest<MaritalStatusDto>
    {
        public string Name { get; init; }
    }

    public class CreateMaritalStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateMaritalStatusCommand, MaritalStatusDto>
    {
        public async Task<MaritalStatusDto> Handle(CreateMaritalStatusCommand request, CancellationToken cancellationToken)
        {
            var maritalStatus = new MaritalStatus
            {
                Name = request.Name
            };

            maritalStatus.AddDomainEvent(new MaritalStatusCreatedEvent(maritalStatus));

            await unitOfWork.MaritalStatusRepository.InsertAsync(maritalStatus, cancellationToken);

            await unitOfWork.SaveAsync(cancellationToken);

            return mapper.Map<MaritalStatusDto>(maritalStatus);
        }
    }
}
