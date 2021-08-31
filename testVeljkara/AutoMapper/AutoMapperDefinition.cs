using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testVeljkara.DbLayer;
using testVeljkara.Dtos;

namespace testVeljkara.AutoMapper
{
    public class AutoMapperDefinition : Profile
    {
        public AutoMapperDefinition()
        {
            CreateMap<User, GetTokenDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<DataBase64, DataDto>().ReverseMap();
        }
    }
}
