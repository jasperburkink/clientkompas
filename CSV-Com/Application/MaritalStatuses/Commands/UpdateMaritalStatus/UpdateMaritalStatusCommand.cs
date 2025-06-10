using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Application.MaritalStatuses.Queries.GetMaritalStatus;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;

namespace Application.MaritalStatuses.Commands.UpdateMaritalStatus
{
    [Authorize(Policy = Policies.MaritalStatusManagement)]
    public record UpdateMaritalStatusCommand : IRequest<MaritalStatusDto>
    {
        public int Id { get; init; }

        public string Name { get; set; }
    }

    public class UpdateMaritalStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateMaritalStatusCommand, MaritalStatusDto>
    {
        public async Task<MaritalStatusDto> Handle(UpdateMaritalStatusCommand request, CancellationToken cancellationToken)
        {
            var maritalStatus = await unitOfWork.MaritalStatusRepository.GetByIDAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(MaritalStatus), request.Id);

            maritalStatus.Name = request.Name;

            await unitOfWork.SaveAsync(cancellationToken);

            return mapper.Map<MaritalStatusDto>(maritalStatus);
        }
    }
}
