using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Application.Organizations.Dtos;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Domain.CVS.ValueObjects;

namespace Application.Organizations.Commands.UpdateOrganization
{
    [Authorize(Policy = Policies.OrganizationManagement)]
    public record UpdateOrganizationCommand : IRequest<OrganizationDto>
    {
        public int Id { get; init; }

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

        public string IBANNumber { get; set; }

        public string BIC { get; set; }
    }

    public class UpdateOrganizationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateOrganizationCommand, OrganizationDto>
    {
        public async Task<OrganizationDto> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var organization = await unitOfWork.OrganizationRepository.GetByIDAsync(request.Id, includeProperties: "", cancellationToken)
                ?? throw new NotFoundException(nameof(Organization), request.Id);

            var existingOrganizationWithKVKNumber = await unitOfWork.OrganizationRepository.GetByKVKNumberAsync(request.KVKNumber, cancellationToken);

            if (existingOrganizationWithKVKNumber != null && existingOrganizationWithKVKNumber.Id != request.Id)
            {
                throw new ItemAlreadyExistsException($"KVKNumber {request.KVKNumber} is already linked to another organization.");
            }

            organization.OrganizationName = request.OrganizationName;

            organization.VisitAddress = Address.From(request.VisitStreetName, request.VisitHouseNumber, request.VisitHouseNumberAddition, request.VisitPostalCode, request.VisitResidence);

            organization.PostAddress = Address.From(request.PostStreetName, request.PostHouseNumber, request.PostHouseNumberAddition, request.PostPostalCode, request.PostResidence);

            organization.InvoiceAddress = Address.From(request.InvoiceStreetName, request.InvoiceHouseNumber, request.InvoiceHouseNumberAddition, request.InvoicePostalCode, request.InvoiceResidence);

            organization.ContactPersonName = request.ContactPersonName;

            organization.ContactPersonFunction = request.ContactPersonFunction;

            organization.ContactPersonTelephoneNumber = request.ContactPersonTelephoneNumber;

            organization.ContactPersonMobilephoneNumber = request.ContactPersonMobilephoneNumber;

            organization.ContactPersonEmailAddress = request.ContactPersonEmailAddress;

            organization.PhoneNumber = request.PhoneNumber;

            organization.Website = request.Website;

            organization.EmailAddress = request.EmailAddress;

            organization.KVKNumber = request.KVKNumber;

            organization.BTWNumber = request.BTWNumber;

            organization.IBANNumber = request.IBANNumber;

            organization.BIC = request.BIC;

            await unitOfWork.OrganizationRepository.UpdateAsync(organization);

            await unitOfWork.SaveAsync(cancellationToken);

            return mapper.Map<OrganizationDto>(organization);
        }
    }
}
