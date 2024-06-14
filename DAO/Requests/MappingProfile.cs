using AutoMapper;
using BusinessObject.Models;
using DAO.Requests;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DentistDetail, DentistDetailDTO>().ReverseMap();
    }
}
