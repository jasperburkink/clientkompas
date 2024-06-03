using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;

namespace Application.Organizations.Dtos
{
    public class OrganizationDto : IMapFrom<Organization>
    {
        public int Id { get; set; }

        public string OrganizationName { get; set; }

        public string VisitStreetName { get; set; }

        public int VisitHouseNumber { get; set; }

        public string VisitHouseNumberAddition { get; set; }

        public string VisitPostalCode { get; set; }

        public string VisitResidence { get; set; }

        public string InvoiceStreetName { get; set; }

        public int InvoiceHouseNumber { get; set; }

        public string InvoiceHouseNumberAddition { get; set; }

        public string InvoicePostalCode { get; set; }

        public string InvoiceResidence { get; set; }

        public string PostStreetName { get; set; }

        public int PostHouseNumber { get; set; }

        public string PostHouseNumberAddition { get; set; }

        public string PostPostalCode { get; set; }

        public string PostResidence { get; set; }

        public string ContactPersonName { get; set; }

        public string ContactPersonFunction { get; set; }

        public string ContactPersonTelephoneNumber { get; set; }

        public string ContactPersonMobilephoneNumber { get; set; }

        public string ContactPersonEmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Website { get; set; }

        public string EmailAddress { get; set; }

        public string KVKNumber { get; set; }

        public string BTWNumber { get; set; }

        public string IBANNumber { get; set; }

        public string BIC { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Organization, OrganizationDto>()
                .ForMember(cDto => cDto.VisitStreetName, address => address.MapFrom(c => c.VisitAddress.StreetName))
                .ForMember(cDto => cDto.VisitHouseNumber, address => address.MapFrom(c => c.VisitAddress.HouseNumber))
                .ForMember(cDto => cDto.VisitHouseNumberAddition, address => address.MapFrom(c => c.VisitAddress.HouseNumberAddition))
                .ForMember(cDto => cDto.VisitPostalCode, address => address.MapFrom(c => c.VisitAddress.PostalCode))
                .ForMember(cDto => cDto.VisitResidence, address => address.MapFrom(c => c.VisitAddress.Residence))
                .ForMember(cDto => cDto.InvoiceStreetName, address => address.MapFrom(c => c.InvoiceAddress.StreetName))
                .ForMember(cDto => cDto.InvoiceHouseNumber, address => address.MapFrom(c => c.InvoiceAddress.HouseNumber))
                .ForMember(cDto => cDto.InvoiceHouseNumberAddition, address => address.MapFrom(c => c.InvoiceAddress.HouseNumberAddition))
                .ForMember(cDto => cDto.InvoicePostalCode, address => address.MapFrom(c => c.InvoiceAddress.PostalCode))
                .ForMember(cDto => cDto.InvoiceResidence, address => address.MapFrom(c => c.InvoiceAddress.Residence))
                .ForMember(cDto => cDto.PostStreetName, address => address.MapFrom(c => c.PostAddress.StreetName))
                .ForMember(cDto => cDto.PostHouseNumber, address => address.MapFrom(c => c.PostAddress.HouseNumber))
                .ForMember(cDto => cDto.PostHouseNumberAddition, address => address.MapFrom(c => c.PostAddress.HouseNumberAddition))
                .ForMember(cDto => cDto.PostPostalCode, address => address.MapFrom(c => c.PostAddress.PostalCode))
                .ForMember(cDto => cDto.PostResidence, address => address.MapFrom(c => c.PostAddress.Residence));
        }
    }
}
