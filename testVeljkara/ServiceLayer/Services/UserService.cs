using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ResponseWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testVeljkara.ApplicationConstants;
using testVeljkara.DbLayer;
using testVeljkara.Dtos;
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

        public async Task<ResponseWrapper<UserDto>> FindByUsernameAsync(string username, string password)
        {
            var userDB = await _context.Users
                .FirstOrDefaultAsync(x => x.Username.Equals(username));

            if (userDB != null)
            {
                if (!VerifyPassword(password, userDB.PasswordHash, userDB.PasswordSalt))
                {
                    return ResponseWrapper<UserDto>.Error(AppConstants.WrongUsernameOrPassword);
                }

                var user = _mapper.Map<UserDto>(userDB);
                return ResponseWrapper<UserDto>.Success(user);
            }

            return ResponseWrapper<UserDto>.Error(AppConstants.WrongUsernameOrPassword);
        }

        public async Task<ResponseWrapper<UserDto>> Insert(UserDto user)
        {
            try
            {
                bool usernameExist = _context.Users.Any(x => x.Username.Equals(user.Username));
                if (usernameExist)
                {
                    return ResponseWrapper<UserDto>.Error(AppConstants.UsernameAlreadyExist);
                }

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

                User userDB = _mapper.Map<User>(user);
                userDB.Id = Guid.NewGuid();
                userDB.PasswordHash = passwordHash;
                userDB.PasswordSalt = passwordSalt;
                _context.Users.Add(userDB);
                await _context.SaveChangesAsync();
                user = _mapper.Map<UserDto>(userDB);
                return ResponseWrapper<UserDto>.Success(user);
            }
            catch (Exception)
            {
                return ResponseWrapper<UserDto>.Error(AppConstants.ErrorSavingUser);
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
