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

namespace Application.BenefitForms.Commands.CreateBenefitForm
{
        public record CreateBenefitFormCommand : IRequest<BenefitForm>
        {
            public int Id { get; init; }
            public string Name { get; init; }
        }
        public class CreateMaritalStatusCommandHandler : IRequestHandler<CreateBenefitFormCommand, BenefitForm>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CreateMaritalStatusCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<BenefitForm> Handle(CreateBenefitFormCommand request, CancellationToken cancellationToken)
            {
                var benefitForm = new BenefitForm
                {
                    Id = request.Id,
                    Name = request.Name
                };
                benefitForm.AddDomainEvent(new BenefitFormCreatedEvent(benefitForm));
                await _unitOfWork.BenefitFormRepository.InsertAsync(benefitForm, cancellationToken);
                await _unitOfWork.SaveAsync(cancellationToken);
                return benefitForm;
            }
        }
    }



