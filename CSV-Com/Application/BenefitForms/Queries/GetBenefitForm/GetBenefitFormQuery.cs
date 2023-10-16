using Application.BenefitForms.Queries;
using Application.BenefitForms.Queries.GetBenefitForm;
using Application.Common.Interfaces.CVS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.CVS.Enums;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace Application.BenefitForms.Queries.GetBenefitForm
{
    public record GetBenefitFormQuery : IRequest<IEnumerable<BenefitFormDto>> { }

    public class GetBenefitFormQueryHandler : IRequestHandler<GetBenefitFormQuery, IEnumerable<BenefitFormDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetBenefitFormQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BenefitFormDto>> Handle(GetBenefitFormQuery request, CancellationToken cancellationToken)
        {
            return (await _unitOfWork.BenefitFormRepository.GetAsync())
               .AsQueryable()
               .ProjectTo<BenefitFormDto>(_mapper.ConfigurationProvider);
        }
    }
}