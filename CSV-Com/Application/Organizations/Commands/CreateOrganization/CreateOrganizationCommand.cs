using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Organizations.Dtos;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Events;
using Domain.CVS.ValueObjects;
using MediatR;

namespace Application.Organizations.Commands.CreateOrganization
{
    public record CreateOrganizationCommand : IRequest<OrganizationDto>
    {
        public string OrganizationName { get; set; }

        public string VisitStreetName { get; set; }

        public int VisitHouseNumber { get; set; }

        public string VisitHouseNumberAddition { get; set; }

        public string VisitPostalCode { get; set; }

        public string VisitResidence { get; set; }

        public string InvoiceStreetName { get; set; }

        public int InvoiceHouseNumber { get; set; }

        public string InvoiceHouseNumberAddition { get; set; }

        public string InvoicePostalCode { get; set; }

        public string InvoiceResidence { get; set; }

        public string PostStreetName { get; set; }

        public int PostHouseNumber { get; set; }

        public string PostHouseNumberAddition { get; set; }

        public string PostPostalCode { get; set; }

        public string PostResidence { get; set; }

        public string ContactPersonName { get; set; }

        public string ContactPersonFunction { get; set; }

        public string ContactPersonTelephoneNumber { get; set; }

        public string ContactPersonMobilephoneNumber { get; set; }

        public string ContactPersonEmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Website { get; set; }

        public string EmailAddress { get; set; }

        public string KVKNumber { get; set; }

        public string BTWNumber { get; set; }

        public ICollection<WorkingContractOrganizationDto> WorkingContracts { get; set; }

        public string IBANNumber { get; set; }

        public string BIC { get; set; }
    }

    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, OrganizationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateOrganizationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrganizationDto> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var existingOrganizationWithKVKNumber = await _unitOfWork.OrganizationRepository.GetByKVKNumberAsync(request.KVKNumber, cancellationToken);
            if (existingOrganizationWithKVKNumber != null)
            {
                throw new ItemAlreadyExistsException($"Organization with KVKNumber {request.KVKNumber} already exists.");
            }

            var organization = new Organization
            {
                OrganizationName = request.OrganizationName,
                VisitAddress = Address.From(request.VisitStreetName, request.VisitHouseNumber, request.VisitHouseNumberAddition, request.VisitPostalCode, request.VisitResidence),
                InvoiceAddress = Address.From(request.InvoiceStreetName, request.InvoiceHouseNumber, request.InvoiceHouseNumberAddition, request.InvoicePostalCode, request.InvoiceResidence),
                PostAddress = Address.From(request.PostStreetName, request.PostHouseNumber, request.PostHouseNumberAddition, request.PostPostalCode, request.PostResidence),
                ContactPersonName = request.ContactPersonName,
                ContactPersonFunction = request.ContactPersonFunction,
                ContactPersonTelephoneNumber = request.ContactPersonTelephoneNumber,
                ContactPersonMobilephoneNumber = request.ContactPersonMobilephoneNumber,
                ContactPersonEmailAddress = request.ContactPersonEmailAddress,
                PhoneNumber = request.PhoneNumber,
                Website = request.Website,
                EmailAddress = request.EmailAddress,
                KVKNumber = request.KVKNumber,
                BTWNumber = request.BTWNumber,
                IBANNumber = request.IBANNumber,
                BIC = request.BIC
            };

            var workingContracts = new List<WorkingContract>();
            foreach (var dto in request.WorkingContracts)
            {
                var client = await _unitOfWork.ClientRepository.GetByIDAsync(dto.ClientId, cancellationToken);
                var workingContract = dto.ToDomainModel(_mapper, organization, client);
                workingContracts.Add(workingContract);
            }

            await _unitOfWork.OrganizationRepository.InsertAsync(organization, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            organization.AddDomainEvent(new OrganizationCreatedEvent(organization));

            return _mapper.Map<OrganizationDto>(organization);
        }
    }
}
