using Application.Common.Interfaces.CVS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.MaritalStatuses.Queries.GetMaritalStatus
{
    public record GetMaritalStatusQuery : IRequest<IEnumerable<MaritalStatusDto>> { }

    public class GetMaritalStatusQueryHandler : IRequestHandler<GetMaritalStatusQuery, IEnumerable<MaritalStatusDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetMaritalStatusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MaritalStatusDto>> Handle(GetMaritalStatusQuery request, CancellationToken cancellationToken)
        {
            return (await _unitOfWork.MaritalStatusRepository.GetAsync())
                .AsQueryable()
                .ProjectTo<MaritalStatusDto>(_mapper.ConfigurationProvider);
        }
    }
}
