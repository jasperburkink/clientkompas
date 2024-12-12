using Application.Common.Models;
using AutoMapper;

namespace EmailModule
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmailMessageDto, EmailMessage>();
        }
    }
}
