using Application.BenefitForms.Queries.GetBenefitForm;
using Application.Clients.Dtos;
using Application.Common.Interfaces.CVS;
using Application.Diagnoses.Queries.GetDiagnosis;
using Application.DriversLicences.Queries;
using Application.MaritalStatuses.Queries.GetMaritalStatus;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.ValueObjects;

namespace Application.Clients.Commands.UpdateClient
{
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

    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, ClientDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ClientDto> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.Id, includeProperties: "DriversLicences,BenefitForms,Diagnoses,EmergencyPeople,WorkingContracts,MaritalStatus", cancellationToken)
                ?? throw new NotFoundException(nameof(Client), request.Id);

            var benefitFormIds = request.BenefitForms.Select(x => x.Id).ToList();
            var benefitForms = (await _unitOfWork.BenefitFormRepository.GetAsync(bf => benefitFormIds.Contains(bf.Id))).ToList();

            var diagnosisIds = request.Diagnoses.Select(x => x.Id).ToList();
            var diagnoses = (await _unitOfWork.DiagnosisRepository.GetAsync(d => diagnosisIds.Contains(d.Id))).ToList();

            var driversLicenceIds = request.DriversLicences.Select(x => x.Id).ToList();
            var driversLicences = (await _unitOfWork.DriversLicenceRepository.GetAsync(dl => driversLicenceIds.Contains(dl.Id))).ToList();

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

            client.BenefitForms = benefitForms;

            client.DriversLicences = driversLicences;

            client.Diagnoses = diagnoses;

            client.EmergencyPeople = request.EmergencyPeople.Select(a => a.ToDomainModel(_mapper, client)).ToList();

            client.WorkingContracts = request.WorkingContracts.Select(a => a.ToDomainModel(_mapper, client)).ToList();

            MaritalStatus? maritalStatus = null;

            if (request.MaritalStatus != null)
            {
                maritalStatus = (await _unitOfWork.MaritalStatusRepository.GetAsync(a => a.Id == request.MaritalStatus.Id))?.SingleOrDefault()
                  ?? throw new NotFoundException(nameof(MaritalStatus), request.MaritalStatus);
            }

            client.MaritalStatus = maritalStatus;

            client.Remarks = request.Remarks;

            await _unitOfWork.ClientRepository.UpdateAsync(client);

            await _unitOfWork.SaveAsync(cancellationToken);

            return _mapper.Map<ClientDto>(client);
        }
    }
}

