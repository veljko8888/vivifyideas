using ResponseWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testVeljkara.Dtos;

namespace testVeljkara.ServiceLayer.Interfaces
{
    public interface IUserService
    {
        Task<ResponseWrapper<UserDto>> Insert(UserDto user);
        Task<ResponseWrapper<UserDto>> FindByUsernameAsync(string username, string password);
    }
}
