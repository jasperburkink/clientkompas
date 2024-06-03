using Application.BenefitForms.Queries.GetBenefitForm;
using Application.Clients.Dtos;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Diagnoses.Queries.GetDiagnosis;
using Application.DriversLicences.Queries;
using Application.MaritalStatuses.Queries.GetMaritalStatus;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.Events;
using Domain.CVS.ValueObjects;
using MediatR;

namespace Application.Clients.Commands.CreateClient
{
    public record CreateClientCommand : IRequest<ClientDto>
    {
        public string FirstName { get; init; }

        public string Initials { get; init; }

        public string PrefixLastName { get; init; }

        public string LastName { get; init; }

        public Gender Gender { get; init; }

        public string StreetName { get; init; }

        public int HouseNumber { get; init; }

        public string HouseNumberAddition { get; init; }

        public string PostalCode { get; init; }

        public string Residence { get; init; }

        public string TelephoneNumber { get; init; }

        public DateOnly DateOfBirth { get; set; }

        public string EmailAddress { get; set; }

        public MaritalStatusDto? MaritalStatus { get; set; }

        public ICollection<BenefitFormDto> BenefitForms { get; set; }

        public ICollection<DriversLicenceDto> DriversLicences { get; set; }

        public ICollection<DiagnosisDto> Diagnoses { get; set; }

        public ICollection<EmergencyPersonDto> EmergencyPeople { get; set; }

        public ICollection<ClientWorkingContractDto> WorkingContracts { get; set; }

        public string Remarks { get; set; }
    }

    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, ClientDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ClientDto> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var benefitFormIds = request.BenefitForms.Select(x => x.Id).ToList();
            var benefitForms = (await _unitOfWork.BenefitFormRepository.GetAsync(bf => benefitFormIds.Contains(bf.Id))).ToList();

            var diagnosisIds = request.Diagnoses.Select(x => x.Id).ToList();
            var diagnoses = (await _unitOfWork.DiagnosisRepository.GetAsync(d => diagnosisIds.Contains(d.Id))).ToList();

            var driversLicenceIds = request.DriversLicences.Select(x => x.Id).ToList();
            var driversLicences = (await _unitOfWork.DriversLicenceRepository.GetAsync(dl => driversLicenceIds.Contains(dl.Id))).ToList();

            MaritalStatus? maritalStatus = null;
            if (request.MaritalStatus != null)
            {
                maritalStatus = (await _unitOfWork.MaritalStatusRepository.GetAsync(a => a.Id == request.MaritalStatus.Id))?.SingleOrDefault()
                  ?? throw new NotFoundException(nameof(MaritalStatus), request.MaritalStatus);
            }

            var client = new Client
            {
                FirstName = request.FirstName,
                Initials = request.Initials,
                PrefixLastName = request.PrefixLastName,
                LastName = request.LastName,
                Gender = request.Gender,
                Address = Address.From(request.StreetName, request.HouseNumber, request.HouseNumberAddition, request.PostalCode, request.Residence),
                TelephoneNumber = request.TelephoneNumber,
                DateOfBirth = request.DateOfBirth,
                EmailAddress = request.EmailAddress,
                BenefitForms = benefitForms,
                MaritalStatus = maritalStatus,
                DriversLicences = driversLicences,
                Diagnoses = diagnoses,
                Remarks = request.Remarks
            };

            client.EmergencyPeople = request.EmergencyPeople.Select(a => a.ToDomainModel(_mapper, client)).ToList();

            client.WorkingContracts = request.WorkingContracts.Select(a => a.ToDomainModel(_mapper, client)).ToList();

            await _unitOfWork.ClientRepository.InsertAsync(client, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            client.AddDomainEvent(new ClientCreatedEvent(client));

            return _mapper.Map<ClientDto>(client);
        }
    }
}
