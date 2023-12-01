using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;


namespace Application.Clients.Queries.GetClients
{
    public class ClientDto : IMapFrom<Client>
    {
        private const char SeperatorChar = ',';

        public int IdentificationNumber { get; set; }

        public string FirstName { get; set; }

        public string Initials { get; set; }

        public string PrefixLastName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string StreetName { get; set; }

        public int HouseNumber { get; set; }

        public string HouseNumberAddition { get; set; }

        public string PostalCode { get; set; }

        public string Residence { get; set; }

        public string TelephoneNumber { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string EmailAddress { get; set; }

        public string MaritalStatus { get; set; }

        public virtual ICollection<EmergencyPersonDto> EmergencyPeople { get; set; }

        public string BenefitForm { get; set; }

        public virtual ICollection<WorkingContractDto> WorkingContracts { get; set; }

        public string Remarks { get; set; }
        public void Mapping(Profile profile)
        {
            // TODO: Get the right text value for the enum values. Depends on language user.
            profile.CreateMap<Client, ClientDto>()
                .ForMember(cDto => cDto.MaritalStatus, ms => ms.MapFrom(c => c.MaritalStatus.Name))
                .ForMember(cDto => cDto.Gender, s => s.MapFrom(c => Enum.GetName(typeof(Gender), c.Gender)))
                .ForMember(cDto => cDto.BenefitForm, ms => ms.MapFrom(c => c.BenefitForm.Name));
        }
    }
}
