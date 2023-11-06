using Application.Common.Interfaces.CVS;
using Application.BenefitForms.Queries.GetBenefitForm;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;

namespace Application.BenefitForms.Commands.CreateBenefitForm
{
        public record CreateBenefitFormCommand : IRequest<BenefitFormDto>
        {
            public string Name { get; init; }
        }

        public class CreateMaritalStatusCommandHandler : IRequestHandler<CreateBenefitFormCommand, BenefitFormDto>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public CreateMaritalStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<BenefitFormDto> Handle(CreateBenefitFormCommand request, CancellationToken cancellationToken)
            {

                var benefitForm = new BenefitForm
                {
                    Name = request.Name
                };


                benefitForm.AddDomainEvent(new BenefitFormCreatedEvent(benefitForm));

                await _unitOfWork.BenefitFormRepository.InsertAsync(benefitForm, cancellationToken);

                await _unitOfWork.SaveAsync(cancellationToken);

                return _mapper.Map<BenefitFormDto>(benefitForm);
            }
        }
    }



