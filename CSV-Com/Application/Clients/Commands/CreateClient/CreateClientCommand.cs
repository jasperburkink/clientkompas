using Application.Clients.Dtos;
using Application.Common.Interfaces.CVS;
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

        public int? MaritalStatus { get; set; }

        public int[] BenefitForms { get; set; }

        public int[] DriversLicences { get; set; }

        public int[] Diagnoses { get; set; }

        public EmergencyPersonDto[] EmergencyPeople { get; set; }

        public WorkingContractDto[] WorkingContracts { get; set; }

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
            var benefitForms = new List<BenefitForm>(await _unitOfWork.BenefitFormRepository.GetAsync(bfdb => request.BenefitForms.Any(bfr => bfdb.Id.Equals(bfr))));

            var maritalStatus = request.MaritalStatus != null
                ? (await _unitOfWork.MaritalStatusRepository.GetAsync(a => a.Id == request.MaritalStatus))?.SingleOrDefault()
                : null;

            var driversLicences = new List<DriversLicence>(await _unitOfWork.DriversLicenceRepository.GetAsync(dldb => request.DriversLicences.Any(dlr => dldb.Id.Equals(dlr))));

            var diagnoses = new List<Diagnosis>(await _unitOfWork.DiagnosisRepository.GetAsync(ddb => request.Diagnoses.Any(dr => ddb.Id.Equals(dr))));

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

            foreach (var emergencyPersonRequest in request.EmergencyPeople)
            {
                var emergencyPerson = emergencyPersonRequest.ToDomainModel(_mapper, client);
                client.EmergencyPeople.Add(emergencyPerson);
            }

            foreach (var workingContractRequest in request.WorkingContracts)
            {
                var workingContract = workingContractRequest.ToDomainModel(_mapper, client);
                client.WorkingContracts.Add(workingContract);
            }

            await _unitOfWork.ClientRepository.InsertAsync(client, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            client.AddDomainEvent(new ClientCreatedEvent(client));

            return _mapper.Map<ClientDto>(client);
        }
    }
}
