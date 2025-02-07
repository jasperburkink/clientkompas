using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.Users.Queries.SearchUsers
{
    public class SearchUsersQueryDto : IMapFrom<User>
    {
        public int Id { get; set; }

        public required string FirstName { get; set; }

        public string? PrefixLastName { get; set; }

        public required string LastName { get; set; }

        public required bool IsDeactivated { get; set; }

        //public void Mapping(Profile profile)
        //{
        //    // TODO: Get the right text value for the enum values. Depends on language user.
        //    profile.CreateMap<Client, GetClientDto>()
        //        .ForMember(cDto => cDto.Gender, s => s.MapFrom(c => Enum.GetName(typeof(Gender), c.Gender)))
        //        .ForMember(cDto => cDto.MaritalStatus, s => s.MapFrom(c => c.MaritalStatus != null ? c.MaritalStatus.Name : string.Empty))
        //        .ForMember(cDto => cDto.DriversLicences, dl => dl.MapFrom(c => string.Join(SeperatorString, c.DriversLicences.OrderBy(dl => dl.Category).Select(dl => dl.Category))))
        //        .ForMember(cDto => cDto.Diagnoses, dDto => dDto.MapFrom(c => string.Join(SeperatorString, c.Diagnoses.OrderBy(d => d.Name).Select(d => d.Name))))
        //        .ForMember(cDto => cDto.BenefitForm, dDto => dDto.MapFrom(c => string.Join(SeperatorString, c.BenefitForms.OrderBy(bf => bf.Name).Select(bf => bf.Name))))
        //        .ForMember(cDto => cDto.StreetName, address => address.MapFrom(c => c.Address.StreetName))
        //        .ForMember(cDto => cDto.HouseNumber, address => address.MapFrom(c => c.Address.HouseNumber))
        //        .ForMember(cDto => cDto.HouseNumberAddition, address => address.MapFrom(c => c.Address.HouseNumberAddition ?? string.Empty))
        //        .ForMember(cDto => cDto.PostalCode, address => address.MapFrom(c => c.Address.PostalCode))
        //        .ForMember(cDto => cDto.Residence, address => address.MapFrom(c => c.Address.Residence))
        //        .ForMember(cDto => cDto.WorkingContracts, workingContract => workingContract.MapFrom(c => c.WorkingContracts));
        //}
    }
}
