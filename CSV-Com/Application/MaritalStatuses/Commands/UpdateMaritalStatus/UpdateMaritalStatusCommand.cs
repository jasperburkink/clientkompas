using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.MaritalStatuses.Queries.GetMaritalStatus;
using AutoMapper;
using Domain.CVS.Domain;
using MediatR;

namespace Application.MaritalStatuses.Commands.UpdateMaritalStatus
{
    public record UpdateMaritalStatusCommand : IRequest<MaritalStatusDto>
    {
        public int Id { get; init; }

        public string Name { get; set; }
    }

    public class UpdateMaritalStatusCommandHandler : IRequestHandler<UpdateMaritalStatusCommand, MaritalStatusDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateMaritalStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MaritalStatusDto> Handle(UpdateMaritalStatusCommand request, CancellationToken cancellationToken)
        {
            var maritalStatus = await _unitOfWork.MaritalStatusRepository.GetByIDAsync(request.Id, cancellationToken);
            if (maritalStatus == null)
            {
                throw new NotFoundException(nameof(MaritalStatus), request.Id);
            }

            maritalStatus.Name = request.Name;

            await _unitOfWork.SaveAsync(cancellationToken);

            return _mapper.Map<MaritalStatusDto>(maritalStatus);
        }
    }
}
