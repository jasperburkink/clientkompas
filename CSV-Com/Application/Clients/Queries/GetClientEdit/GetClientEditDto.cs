using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;

namespace Application.Clients.Queries.GetClientEdit
{
    public class GetClientEditDto : IMapFrom<Client>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string Initials { get; set; }

        public string PrefixLastName { get; set; }

        public string LastName { get; set; }

        public int Gender { get; set; }

        public string StreetName { get; set; }

        public int HouseNumber { get; set; }

        public string HouseNumberAddition { get; set; }

        public string PostalCode { get; set; }

        public string Residence { get; set; }

        public string TelephoneNumber { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string EmailAddress { get; set; }

        public GetClientEditMaritalStatusDto? MaritalStatus { get; set; }

        public virtual ICollection<GetClientEditDriversLicenceDto> DriversLicences { get; set; }

        public virtual ICollection<GetClientEditDiagnosisDto> Diagnoses { get; set; }

        public virtual ICollection<GetClientEditEmergencyPersonDto> EmergencyPeople { get; set; }

        public virtual ICollection<GetClientEditBenefitFormDto> BenefitForms { get; set; }

        public virtual ICollection<GetClientEditWorkingContractDto> WorkingContracts { get; set; }

        public string Remarks { get; set; }

        public void Mapping(Profile profile)
        {
            // TODO: Get the right text value for the enum values. Depends on language user.
            profile.CreateMap<Client, GetClientEditDto>()
                .ForMember(cDto => cDto.MaritalStatus, ms => ms.MapFrom(c => c.MaritalStatus))
                .ForMember(cDto => cDto.Gender, s => s.MapFrom(c => c.Gender))
                .ForMember(cDto => cDto.StreetName, address => address.MapFrom(c => c.Address.StreetName))
                .ForMember(cDto => cDto.HouseNumber, address => address.MapFrom(c => c.Address.HouseNumber))
                .ForMember(cDto => cDto.HouseNumberAddition, address => address.MapFrom(c => c.Address.HouseNumberAddition ?? string.Empty))
                .ForMember(cDto => cDto.PostalCode, address => address.MapFrom(c => c.Address.PostalCode))
                .ForMember(cDto => cDto.Residence, address => address.MapFrom(c => c.Address.Residence));
        }
    }
}
