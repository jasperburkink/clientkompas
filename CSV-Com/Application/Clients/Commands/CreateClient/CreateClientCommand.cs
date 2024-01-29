using Application.Clients.Dtos;
using Application.Common.Exceptions;
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

        public string MaritalStatus { get; set; }

        public string BenefitForm { get; set; }

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
            var benefitFormTask = _unitOfWork.BenefitFormRepository.GetByIDAsync(request.BenefitFormid, cancellationToken);
            var maritalStatusTask = _unitOfWork.MaritalStatusRepository.GetByIDAsync(request.MaritalStatusid, cancellationToken);

            await Task.WhenAll(benefitFormTask, maritalStatusTask);

            var benefitForm = benefitFormTask.Result;
            var maritalStatus = maritalStatusTask.Result;

            
            var benefitForm = (await _unitOfWork.BenefitFormRepository.GetAsync(a => a.Name == request.BenefitForm))?.SingleOrDefault()
                ?? throw new NotFoundException(nameof(BenefitForm), request.BenefitForm);

            var maritalStatus = (await _unitOfWork.MaritalStatusRepository.GetAsync(a => a.Name == request.MaritalStatus))?.SingleOrDefault()
              ?? throw new NotFoundException(nameof(MaritalStatus), request.MaritalStatus);

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
                BenefitForm = benefitForm,
                MaritalStatus = maritalStatus,

                Remarks = request.Remarks
            };

            client.AddDomainEvent(new ClientCreatedEvent(client));

            await _unitOfWork.ClientRepository.InsertAsync(client, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return _mapper.Map<ClientDto>(client);
        }
    }
}
