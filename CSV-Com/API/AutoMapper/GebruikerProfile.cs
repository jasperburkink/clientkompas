using API.ViewModels;
using AutoMapper;
using Domain.CVS.Domain;

namespace API.AutoMapper
{
    public class GebruikerProfile : Profile
    {
        public GebruikerProfile()
        {
            CreateMap<Gebruiker, GebruikerViewModel>().ReverseMap();
        }
    }
}
