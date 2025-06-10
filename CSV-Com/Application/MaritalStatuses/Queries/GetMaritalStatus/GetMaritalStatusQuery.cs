using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.MaritalStatuses.Queries.GetMaritalStatus
{
    [Authorize(Policy = Policies.MaritalStatusRead)]
    public record GetMaritalStatusQuery : IRequest<IEnumerable<MaritalStatusDto>> { }

    public class GetMaritalStatusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetMaritalStatusQuery, IEnumerable<MaritalStatusDto>>
    {
        public async Task<IEnumerable<MaritalStatusDto>> Handle(GetMaritalStatusQuery request, CancellationToken cancellationToken)
        {
            return (await unitOfWork.MaritalStatusRepository.GetAsync())
                .AsQueryable()
                .ProjectTo<MaritalStatusDto>(mapper.ConfigurationProvider);
        }
    }
}
