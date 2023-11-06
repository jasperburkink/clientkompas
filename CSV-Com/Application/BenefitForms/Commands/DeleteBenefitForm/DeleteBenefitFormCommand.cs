using Application.Clients.Commands.CreateClient;
using Application.Clients.Queries.GetClients;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.Events;
using FluentValidation.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Application.BenefitForms.Queries.GetBenefitForm;

namespace Application.BenefitForms.Commands.DeleteBenefitForm
{
    public record DeleteBenefitFormCommand : IRequest<BenefitFormDto>
    {
        public int Id { get; init; }
    }

    public class DeleteMaritalStatusCommandHandler : IRequestHandler<DeleteBenefitFormCommand, BenefitFormDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteMaritalStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BenefitFormDto> Handle(DeleteBenefitFormCommand request, CancellationToken cancellationToken)
        {

            // Check if maritalstatus exists in the database
            var benefitForm = await _unitOfWork.BenefitFormRepository.GetByIDAsync(request.Id, cancellationToken);
            if (benefitForm == null)
            {
                throw new NotFoundException(nameof(MaritalStatus), request.Id);
            }

            // Check if there's any client that uses the maritalstatus
            var clients = await _unitOfWork.ClientRepository.GetAsync(c => c.BenefitForm.Id.Equals(request.Id));
            if (clients.Any())
            {
                throw new DomainObjectInUseExeption(nameof(BenefitForm), request.Id, nameof(Client), clients.Select(c => (object)c.Id));
            }

            await _unitOfWork.BenefitFormRepository.DeleteAsync(benefitForm);

            await _unitOfWork.SaveAsync(cancellationToken);

            return _mapper.Map<BenefitFormDto>(benefitForm);
        }
    }
}

