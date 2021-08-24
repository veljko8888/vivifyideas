using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ResponseWrapper;
using testVeljkara.DbLayer;
using testVeljkara.Dtos;
using testVeljkara.ServiceLayer.Interfaces;

namespace testVeljkara.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<TestController> _logger;
        private readonly AppSettings _appSettings;
        private readonly ILinkService _linkService;
        private readonly IUserService _userService;

        public TestController(
            ILogger<TestController> logger,
            ApplicationDbContext dbContext,
            IOptions<AppSettings> appSettings,
            ILinkService linkService,
            IUserService userService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
            _linkService = linkService;
            _userService = userService;
        }

        [HttpPost]
        [Route("GetToken")]
        public async Task<IActionResult> Login(GetTokenDto tokenDto)
        {
            ResponseWrapper<UserDto> result = await _userService.FindByUsernameAsync(tokenDto.Username, tokenDto.Password);
            if (result != null && !result.HasError)
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", result.Data.Id.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(120),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
            {
                return BadRequest("Wrong Data");
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateUser(UserDto userDto)
        {
            var insert = await _userService.Insert(userDto);
            if (insert.HasError)
            {
                return BadRequest(insert.ErrorMessages);
            }
            else
            {
                return Ok(insert.Data);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("SaveLink")]
        public async Task<IActionResult> SaveLink(SaveLinkDto saveLinkDto)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            saveLinkDto.UserId = new Guid(userId);
            var insertLink = await _linkService.Insert(saveLinkDto);
            if (insertLink.HasError)
            {
                return BadRequest(insertLink.ErrorMessages);
            }
            else
            {
                return Ok(insertLink.Data);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("DeleteLink")]
        public async Task<IActionResult> DeleteLink(SaveLinkDto saveLinkDto)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            saveLinkDto.UserId = new Guid(userId);
            var deleteLink = await _linkService.Delete(saveLinkDto);
            if (deleteLink.HasError)
            {
                return BadRequest(deleteLink.ErrorMessages);
            }
            else
            {
                return Ok(deleteLink.Data);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("CountSame")]
        public async Task<IActionResult> CountSame(SaveLinkDto saveLinkDto)
        {
            var countSame = await _linkService.CountUsers(saveLinkDto);
            if (countSame.HasError)
            {
                return BadRequest(countSame.ErrorMessages);
            }
            else
            {
                return Ok(countSame.Data);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("link")]
        public async Task<IActionResult> RedirectTo([FromQuery] string shortUrl)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var redirectURL = _linkService.RedirectFromShort(shortUrl, new Guid(userId));
            if (!string.IsNullOrEmpty(redirectURL))
            {
                return Ok(redirectURL);
            }
            else
            {
                return NotFound("no url connected to provided short url.");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("AuthTest")]
        public async Task<IActionResult> TestAuthorisation()
        {
            return Ok("aaa");
        }

        [HttpGet]
        [Authorize]
        [Route("QueryTest")]
        public async Task<IActionResult> QueryTest([FromQuery] string param1, [FromQuery] string param2)
        {
            return Ok("aaa");
        }
    }
}
