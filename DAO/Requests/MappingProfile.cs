using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.Models;

namespace DAO.Requests
{
    public class MappingProfile :  Profile
    {
          public MappingProfile()
    {
        CreateMap<DentistDetail, DentistDetailDTO>();
    }
    }
}