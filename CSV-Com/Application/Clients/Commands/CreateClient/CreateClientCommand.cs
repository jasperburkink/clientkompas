using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.Events;
using MediatR;

namespace Application.Clients.Commands.CreateClient
{
    public record CreateClientCommand : IRequest<int>
    {
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

    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateClientCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var client = new Client
            {
                IdentificationNumber = request.IdentificationNumber,
                FirstName = request.FirstName,
                Initials = request.Initials,
                PrefixLastName = request.PrefixLastName,
                LastName = request.LastName,
                Gender = request.Gender,
                StreetName = request.StreetName,
                HouseNumber = request.HouseNumber,
                HouseNumberAddition = request.HouseNumberAddition,
                PostalCode = request.PostalCode,
                Residence = request.Residence,
                TelephoneNumber = request.TelephoneNumber,
                DateOfBirth = request.DateOfBirth,
                EmailAddress = request.EmailAddress,
                MaritalStatus = request.MaritalStatus,
                BenefitForm = request.BenefitForm,
                Remarks = request.Remarks
            };

            client.AddDomainEvent(new ClientCreatedEvent(client));

            await _unitOfWork.ClientRepository.InsertAsync(client, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return client.Id;
        }
    }
}
