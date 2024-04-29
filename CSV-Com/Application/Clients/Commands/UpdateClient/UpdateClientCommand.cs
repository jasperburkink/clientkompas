using Application.Clients.Dtos;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.ValueObjects;
using MediatR;

namespace Application.Clients.Commands.UpdateClient
{
    public record UpdateClientCommand : IRequest<ClientDto>
    {
        public int Id { get; init; }

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

            var benefitForms = (await _unitOfWork.BenefitFormRepository.GetAsync(bf => request.BenefitForms.Contains(bf.Id))).ToList();

            var maritalStatus = (await _unitOfWork.MaritalStatusRepository.GetAsync(a => a.Id == request.MaritalStatus))?.SingleOrDefault()
              ?? throw new NotFoundException(nameof(MaritalStatus), request.MaritalStatus);

            client.FirstName = request.FirstName;

            client.Initials = request.Initials;

            client.PrefixLastName = request.PrefixLastName;

            client.LastName = request.LastName;

            client.Gender = request.Gender;

            client.Address = Address.From(request.StreetName, request.HouseNumber, request.HouseNumberAddition, request.PostalCode, request.Residence);

            client.TelephoneNumber = request.TelephoneNumber;

            client.DateOfBirth = request.DateOfBirth;

            client.EmailAddress = request.EmailAddress;

            client.BenefitForms = benefitForms;

            client.DriversLicences = _unitOfWork.DriversLicenceRepository.Get(bf => request.BenefitForms.Contains(bf.Id)).ToList();

            client.Diagnoses = _unitOfWork.DiagnosisRepository.Get(diagnosis => request.Diagnoses.Contains(diagnosis.Id)).ToList();

            client.EmergencyPeople = request.EmergencyPeople.Select(a => a.ToDomainModel(_mapper, client)).ToList();

            client.WorkingContracts = request.WorkingContracts.Select(a => a.ToDomainModel(_mapper, client)).ToList();

            client.MaritalStatus = maritalStatus;

            client.Remarks = request.Remarks;

            await _unitOfWork.ClientRepository.UpdateAsync(client);

            await _unitOfWork.SaveAsync(cancellationToken);

            return _mapper.Map<ClientDto>(client);
        }
    }
}

