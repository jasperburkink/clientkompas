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
using Domain.CVS.ValueObjects;

namespace Application.Clients.Commands.UpdateClient
{
    [Authorize(Policy = Policies.ClientManagement)]
    public record UpdateClientCommand : IRequest<ClientDto>
    {
        public int Id { get; set; }

        public string? FirstName { get; init; }

        public string? Initials { get; init; }

        public string? PrefixLastName { get; init; }

        public string? LastName { get; init; }

        public Gender? Gender { get; init; }

        public string? StreetName { get; init; }

        public int? HouseNumber { get; init; }

        public string? HouseNumberAddition { get; init; }

        public string? PostalCode { get; init; }

        public string? Residence { get; init; }

        public string? TelephoneNumber { get; init; }

        public DateOnly? DateOfBirth { get; set; }

        public string? EmailAddress { get; set; }

        public MaritalStatusDto? MaritalStatus { get; set; }

        public bool? IsInTargetGroupRegister { get; set; }

        public ICollection<BenefitFormDto> BenefitForms { get; set; }

        public ICollection<DriversLicenceDto> DriversLicences { get; set; }

        public ICollection<DiagnosisDto> Diagnoses { get; set; }

        public ICollection<EmergencyPersonDto> EmergencyPeople { get; set; }

        public ICollection<ClientWorkingContractDto> WorkingContracts { get; set; }

        public string? Remarks { get; set; }
    }

    public class UpdateClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateClientCommand, ClientDto>
    {
        public async Task<ClientDto> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await unitOfWork.ClientRepository.GetByIDAsync(request.Id, includeProperties: "DriversLicences,BenefitForms,Diagnoses,EmergencyPeople,WorkingContracts,MaritalStatus", cancellationToken)
                ?? throw new NotFoundException(nameof(Client), request.Id);

            var benefitFormIds = request.BenefitForms.Select(x => x.Id).ToList();
            var benefitForms = (await unitOfWork.BenefitFormRepository.GetAsync())
                .Where(bf => benefitFormIds.Contains(bf.Id));

            var diagnosisIds = request.Diagnoses.Select(x => x.Id).ToList();
            var diagnoses = (await unitOfWork.DiagnosisRepository.GetAsync())
                .Where(d => diagnosisIds.Contains(d.Id));

            var driversLicenceIds = request.DriversLicences.Select(x => x.Id).ToList();
            var driversLicences = (await unitOfWork.DriversLicenceRepository.GetAsync())
                .Where(dl => driversLicenceIds.Contains(dl.Id));

            client.FirstName = request.FirstName;

            client.Initials = request.Initials;

            client.PrefixLastName = request.PrefixLastName;

            client.LastName = request.LastName;

            client.Gender = request.Gender.Value;

            client.Address = Address.From(request.StreetName, request.HouseNumber.Value, request.HouseNumberAddition, request.PostalCode, request.Residence);

            client.TelephoneNumber = request.TelephoneNumber;

            client.DateOfBirth = request.DateOfBirth.Value;

            client.EmailAddress = request.EmailAddress;

            client.IsInTargetGroupRegister = request.IsInTargetGroupRegister.Value;

            client.BenefitForms = [.. benefitForms];

            client.DriversLicences = [.. driversLicences];

            client.Diagnoses = [.. diagnoses];

            client.EmergencyPeople = request.EmergencyPeople.Select(a => a.ToDomainModel(mapper, client)).ToList();

            client.WorkingContracts = request.WorkingContracts.Select(a => a.ToDomainModel(mapper, client)).ToList();

            MaritalStatus? maritalStatus = null;

            if (request.MaritalStatus != null)
            {
                maritalStatus = (await unitOfWork.MaritalStatusRepository.GetAsync(a => a.Id == request.MaritalStatus.Id))?.SingleOrDefault()
                  ?? throw new NotFoundException(nameof(MaritalStatus), request.MaritalStatus);
            }

            client.MaritalStatus = maritalStatus;

            client.Remarks = request.Remarks;

            await unitOfWork.ClientRepository.UpdateAsync(client);

            await unitOfWork.SaveAsync(cancellationToken);

            return mapper.Map<ClientDto>(client);
        }
    }
}

