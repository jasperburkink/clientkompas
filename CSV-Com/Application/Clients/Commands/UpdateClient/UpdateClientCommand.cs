using Application.Clients.Queries.GetClients;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
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

        public MaritalStatus MaritalStatus { get; set; }

        public ICollection<EmergencyPerson> EmergencyPeople { get; set; }

        public ICollection<Domain.CVS.Domain.DriversLicence> DriversLicences { get; set; }

        public ICollection<Diagnosis> Diagnoses { get; set; }

        public BenefitForm BenefitForm { get; set; }

        public ICollection<WorkingContract> WorkingContracts { get; set; }

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
            var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Client), request.Id);

            client.FirstName = request.FirstName;

            client.Initials = request.Initials;

            client.PrefixLastName = request.PrefixLastName;

            client.LastName = request.LastName;

            client.Gender = request.Gender;

            client.StreetName = request.StreetName;

            client.HouseNumber = request.HouseNumber;

            client.HouseNumberAddition = request.HouseNumberAddition;

            client.PostalCode = request.PostalCode;

            client.Residence = request.Residence;

            client.TelephoneNumber = request.TelephoneNumber;

            client.DateOfBirth = request.DateOfBirth;

            client.EmailAddress = request.EmailAddress;

            client.BenefitForm = request.BenefitForm;

            client.EmergencyPeople = request.EmergencyPeople;

            client.MaritalStatus = request.MaritalStatus;

            client.Remarks = request.Remarks;

            client.DriversLicences = request.DriversLicences;

            client.Diagnoses = request.Diagnoses;

            await _unitOfWork.SaveAsync(cancellationToken);

            return _mapper.Map<ClientDto>(client);
        }
    }
}

