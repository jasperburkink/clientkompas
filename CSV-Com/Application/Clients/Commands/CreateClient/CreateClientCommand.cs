using Application.BenefitForms.Queries.GetBenefitForm;
using Application.Clients.Dtos;
using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Application.Diagnoses.Queries.GetDiagnosis;
using Application.DriversLicences.Queries;
using Application.MaritalStatuses.Queries.GetMaritalStatus;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.Events;
using Domain.CVS.ValueObjects;

namespace Application.Clients.Commands.CreateClient
{
    [Authorize(Policy = Policies.ClientManagement)]
    public record CreateClientCommand : IRequest<ClientDto>
    {
        public string? FirstName { get; set; }

        public string? Initials { get; set; }

        public string? PrefixLastName { get; set; }

        public string? LastName { get; set; }

        public Gender? Gender { get; set; }

        public string? StreetName { get; set; }

        public int? HouseNumber { get; set; }

        public string? HouseNumberAddition { get; set; }

        public string? PostalCode { get; set; }

        public string? Residence { get; set; }

        public string? TelephoneNumber { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? EmailAddress { get; set; }

        public MaritalStatusDto? MaritalStatus { get; set; }

        public bool? IsInTargetGroupRegister { get; set; }

        public ICollection<BenefitFormDto> BenefitForms { get; set; }

        public ICollection<DriversLicenceDto> DriversLicences { get; set; }

        public ICollection<DiagnosisDto> Diagnoses { get; set; }

        public ICollection<EmergencyPersonDto> EmergencyPeople { get; set; }

        public ICollection<ClientWorkingContractDto> WorkingContracts { get; set; }

        public string Remarks { get; set; }

        public CreateClientCommand()
        {

        }
    }

    public class CreateClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateClientCommand, ClientDto>
    {
        public async Task<ClientDto> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var benefitFormIds = request.BenefitForms.Select(x => x.Id).ToList();
            var benefitForms = (await unitOfWork.BenefitFormRepository.GetAsync())
                .Where(bf => benefitFormIds.Contains(bf.Id));

            var diagnosisIds = request.Diagnoses.Select(x => x.Id).ToList();
            var diagnoses = (await unitOfWork.DiagnosisRepository.GetAsync())
                .Where(d => diagnosisIds.Contains(d.Id));

            var driversLicenceIds = request.DriversLicences.Select(x => x.Id).ToList();
            var driversLicences = (await unitOfWork.DriversLicenceRepository.GetAsync())
                .Where(dl => driversLicenceIds.Contains(dl.Id));

            MaritalStatus? maritalStatus = null;
            if (request.MaritalStatus != null)
            {
                maritalStatus = (await unitOfWork.MaritalStatusRepository.GetAsync(a => a.Id == request.MaritalStatus.Id))?.SingleOrDefault()
                  ?? throw new NotFoundException(nameof(MaritalStatus), request.MaritalStatus);
            }

            var client = new Client
            {
                FirstName = request.FirstName,
                Initials = request.Initials,
                PrefixLastName = request.PrefixLastName,
                LastName = request.LastName,
                Gender = request.Gender.Value,
                Address = Address.From(request.StreetName, request.HouseNumber.Value, request.HouseNumberAddition, request.PostalCode, request.Residence),
                TelephoneNumber = request.TelephoneNumber,
                DateOfBirth = request.DateOfBirth.Value,
                EmailAddress = request.EmailAddress,
                IsInTargetGroupRegister = request.IsInTargetGroupRegister.Value,
                BenefitForms = [.. benefitForms],
                MaritalStatus = maritalStatus,
                DriversLicences = [.. driversLicences],
                Diagnoses = [.. diagnoses],
                Remarks = request.Remarks,
                EmergencyPeople = [],
                WorkingContracts = []
            };

            client.EmergencyPeople = request.EmergencyPeople.Select(a => a.ToDomainModel(mapper, client)).ToList();

            client.WorkingContracts = request.WorkingContracts.Select(a => a.ToDomainModel(mapper, client)).ToList();

            await unitOfWork.ClientRepository.InsertAsync(client, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);

            client.AddDomainEvent(new ClientCreatedEvent(client));

            return mapper.Map<ClientDto>(client);
        }
    }
}
