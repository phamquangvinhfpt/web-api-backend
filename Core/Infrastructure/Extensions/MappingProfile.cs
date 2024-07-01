using AutoMapper;
using BusinessObject.Models;
using Core.Models.Personal;

namespace Core.Infrastructure.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, UserDetailsDto>().ReverseMap();
        }
    }
}