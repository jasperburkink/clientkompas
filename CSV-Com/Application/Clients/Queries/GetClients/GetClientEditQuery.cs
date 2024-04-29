using Application.Common.Interfaces.CVS;
using AutoMapper;
using MediatR;

namespace Application.Clients.Queries.GetClients
{
    public record GetClientEditQuery : IRequest<GetClientEditDto>
    {
        public int ClientId { get; init; }
    }

    internal class GetClientEditQueryHandler : IRequestHandler<GetClientEditQuery, GetClientEditDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientEditQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetClientEditDto> Handle(GetClientEditQuery request, CancellationToken cancellationToken)
        {
            // TODO: Find a better solution for including properties.
            var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.ClientId, cancellationToken: cancellationToken, includeProperties: "DriversLicences,BenefitForms,Diagnoses,EmergencyPeople,WorkingContracts,MaritalStatus");
            return _mapper.Map<GetClientEditDto>(client);
        }
    }
}
