using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Clients.Queries.GetClients;
using AutoMapper;
using Domain.CVS.Domain;
using MediatR;
using Domain.CVS.Enums;

namespace Application.Clients.Commands.UpdateClient
{
        public record UpdateClientCommand : IRequest<ClientDto>
        {
        public int Id { get; init; }

        public int IdentificationNumber { get; init; }

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

        public BenefitForm BenefitForm { get; set; }

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
                var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.Id, cancellationToken);
                if (client == null)
                {
                    throw new NotFoundException(nameof(Client), request.Id);
                }

                client.IdentificationNumber = request.IdentificationNumber;

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

                client.MaritalStatus = request.MaritalStatus;

                client.BenefitForm = request.BenefitForm;

                client.Remarks = request.Remarks;
                

                await _unitOfWork.SaveAsync(cancellationToken);

                return _mapper.Map<ClientDto>(client);
            }
        }
    }

