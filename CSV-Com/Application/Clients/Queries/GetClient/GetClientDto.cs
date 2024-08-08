using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;

namespace Application.Clients.Queries.GetClient
{
    public class GetClientDto : IMapFrom<Client>
    {
        private const string SeperatorString = ", ";

        public int Id { get; set; }

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

        public required bool IsInTargetGroupRegister { get; set; }

        public string MaritalStatus { get; set; }

        public string DriversLicences { get; set; }

        public virtual ICollection<GetClientEmergencyPersonDto> EmergencyPeople { get; set; }

        public virtual string Diagnoses { get; set; }

        public string BenefitForm { get; set; }

        public virtual ICollection<GetClientWorkingContractDto> WorkingContracts { get; set; }

        public string Remarks { get; set; }

        public void Mapping(Profile profile)
        {
            // TODO: Get the right text value for the enum values. Depends on language user.
            profile.CreateMap<Client, GetClientDto>()
                .ForMember(cDto => cDto.Gender, s => s.MapFrom(c => Enum.GetName(typeof(Gender), c.Gender)))
                .ForMember(cDto => cDto.MaritalStatus, s => s.MapFrom(c => c.MaritalStatus != null ? c.MaritalStatus.Name : string.Empty))
                .ForMember(cDto => cDto.DriversLicences, dl => dl.MapFrom(c => string.Join(SeperatorString, c.DriversLicences.OrderBy(dl => dl.Category).Select(dl => dl.Category))))
                .ForMember(cDto => cDto.Diagnoses, dDto => dDto.MapFrom(c => string.Join(SeperatorString, c.Diagnoses.OrderBy(d => d.Name).Select(d => d.Name))))
                .ForMember(cDto => cDto.BenefitForm, dDto => dDto.MapFrom(c => string.Join(SeperatorString, c.BenefitForms.OrderBy(bf => bf.Name).Select(bf => bf.Name))))
                .ForMember(cDto => cDto.StreetName, address => address.MapFrom(c => c.Address.StreetName))
                .ForMember(cDto => cDto.HouseNumber, address => address.MapFrom(c => c.Address.HouseNumber))
                .ForMember(cDto => cDto.HouseNumberAddition, address => address.MapFrom(c => c.Address.HouseNumberAddition ?? string.Empty))
                .ForMember(cDto => cDto.PostalCode, address => address.MapFrom(c => c.Address.PostalCode))
                .ForMember(cDto => cDto.Residence, address => address.MapFrom(c => c.Address.Residence))
                .ForMember(cDto => cDto.WorkingContracts, workingContract => workingContract.MapFrom(c => c.WorkingContracts));
        }
    }
}
