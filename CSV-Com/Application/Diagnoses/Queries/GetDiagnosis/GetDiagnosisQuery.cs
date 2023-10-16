using Application.Diagnoses.Queries;
using Application.Diagnoses.Queries.GetDiagnosis;
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
namespace Application.Diagnoses.Queries.GetDiagnosis
{
    public record GetDiagnosisQuery : IRequest<IEnumerable<DiagnosisDto>> { }
        public class GetDiagnosisQueryHandler : IRequestHandler<GetDiagnosisQuery, IEnumerable<DiagnosisDto>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            public GetDiagnosisQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }
            public async Task<IEnumerable<DiagnosisDto>> Handle(GetDiagnosisQuery request, CancellationToken cancellationToken)
            {
                return (await _unitOfWork.DiagnosisRepository.GetAsync())
                   .AsQueryable()
                   .ProjectTo<DiagnosisDto>(_mapper.ConfigurationProvider);
            }
        }
    }