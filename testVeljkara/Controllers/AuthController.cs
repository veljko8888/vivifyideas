using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testVeljkara.Dtos;
using testVeljkara.Helpers;
using testVeljkara.ServiceLayer.Interfaces;

namespace testVeljkara.Controllers
{
    [ApiController]
    [Route("v1/auth")]
    public class AuthController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly AppSettings _appSettings;

        public AuthController(
            IUserService userService,
            IAuthService authService,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _authService = authService;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Generates and returns jwt token which must be used for authorisation on
        /// other API endpoints
        /// </summary>
        /// <param name="tokenDto">token info input object with details</param>
        /// <returns>http response with the token itself or bad request</returns>
        [HttpPost]
        [Route("GetToken")]
        public async Task<IActionResult> Login(GetTokenDto tokenDto)
        {
            Result<UserDto> result = await _userService.FindByUsernameAsync(tokenDto.Username, tokenDto.Password);
            if (result.Status == ResultStatus.Success)
            {
                string token = _authService.GetToken(result.ResultObject.Id, _appSettings);
                return Ok(new { token });
            }
            else
            {
                return BadRequest("Wrong Data");
            }
        }

        /// <summary>
        /// Creates user in the database
        /// </summary>
        /// <param name="userDto">object with the user details information</param>
        /// <returns>http response with eventual data information</returns>
        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser(UserDto userDto)
        {
            Result<UserDto> insertedUser = await _userService.Insert(userDto);
            insertedUser.ActionName = "CreateUser";
            return GenerateResponse(insertedUser);
        }
    }
}
