using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using testVeljkara.ServiceLayer.Interfaces;

namespace testVeljkara.ServiceLayer.Services
{
    public class AuthService : IAuthService
    {
        /// <summary>
        /// Generates and retrieves the token
        /// </summary>
        /// <param name="userId">id of the user for which the token is generated</param>
        /// <param name="appSettings">settings in which jwt secret key is stored for
        /// generating the jwt token</param>
        /// <returns>string representation of the jwt token</returns>
        public string GetToken(int userId, AppSettings appSettings)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", userId.ToString()),
                    }),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}
