using System.Threading.Tasks;
using testVeljkara.Dtos;
using testVeljkara.Helpers;

namespace testVeljkara.ServiceLayer.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserDto>> Insert(UserDto user);
        Task<Result<UserDto>> FindByUsernameAsync(string username, string password);
    }
}
