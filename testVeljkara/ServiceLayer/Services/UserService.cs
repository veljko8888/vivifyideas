using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using testVeljkara.DbLayer;
using testVeljkara.Dtos;
using testVeljkara.Helpers;
using testVeljkara.ServiceLayer.Interfaces;

namespace testVeljkara.ServiceLayer.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        public UserService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> FindByUsernameAsync(string username, string password)
        {
            Result<UserDto> result = new Result<UserDto>();
            User userDB = await _context.Users
                .FirstOrDefaultAsync(x => x.Username.Equals(username));

            if (userDB != null)
            {
                if (!VerifyPassword(password, userDB.PasswordHash, userDB.PasswordSalt))
                {
                    return HelperManager<UserDto>.GenerateErrorServiceResult(
                        ResultStatus.NotFound,
                        "Wrong username or password.",
                        result);
                }

                var user = _mapper.Map<UserDto>(userDB);
                return HelperManager<UserDto>.GenerateServiceSuccessResult(ResultStatus.Success, user, result);
            }

            return HelperManager<UserDto>.GenerateErrorServiceResult(ResultStatus.NotFound, "Wrong username or password.", result);
        }

        public async Task<Result<UserDto>> Insert(UserDto user)
        {
            Result<UserDto> result = new Result<UserDto>();
            try
            {
                bool usernameExist = _context.Users.Any(x => x.Username.Equals(user.Username));
                if (usernameExist)
                {
                    return HelperManager<UserDto>.GenerateErrorServiceResult(
                        ResultStatus.InvalidParameters,
                        "Username already exists.",
                        result);
                }

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

                User userDB = _mapper.Map<User>(user);
                userDB.PasswordHash = passwordHash;
                userDB.PasswordSalt = passwordSalt;
                _context.Users.Add(userDB);
                await _context.SaveChangesAsync();
                user = _mapper.Map<UserDto>(userDB);
                return HelperManager<UserDto>.GenerateServiceSuccessResult(
                        ResultStatus.Created,
                        user,
                        result);
            }
            catch (Exception ex)
            {
                return HelperManager<UserDto>.GenerateErrorServiceResult(
                        ResultStatus.Failure,
                        ex.Message,
                        result);
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // Create hash using password salt.
                for (int i = 0; i < computedHash.Length; i++)
                { // Loop through the byte array
                    if (computedHash[i] != passwordHash[i]) return false; // if mismatch
                }
            }

            return true; //if no mismatches.
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
