using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.BenefitForms.Queries.GetBenefitForm;
using Domain.CVS.Domain;
using MediatR;
using AutoMapper;

namespace Application.BenefitForms.Commands.UpdateBenefitForm
{
    public record UpdateBenefitFormCommand : IRequest<BenefitFormDto>
    {
        public int Id { get; init; }

        public string Name { get; set; }
    }

    public class UpdateBenefitFormCommandHandler : IRequestHandler<UpdateBenefitFormCommand, BenefitFormDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateBenefitFormCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BenefitFormDto> Handle(UpdateBenefitFormCommand request, CancellationToken cancellationToken)
        {

            var benefitForm = await _unitOfWork.BenefitFormRepository.GetByIDAsync(request.Id, cancellationToken);
            if (benefitForm == null)
            {
                throw new NotFoundException(nameof(BenefitForm), request.Id);
            }

            benefitForm.Name = request.Name;

            await _unitOfWork.SaveAsync(cancellationToken);

            return _mapper.Map<BenefitFormDto>(benefitForm);
        }
    }
}
