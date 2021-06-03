using BackEndBDAPP.Models;
using BackEndBDAPP.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;


namespace BackEndBDAPP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequestRateLimit(Name = "Limit Request Number",Seconds = 5)]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UniKPContext _context = new UniKPContext();
        private readonly ITokenService _tokenService;
        public LoginController(IConfiguration config, UniKPContext context, ITokenService tokenService)
        {
            _config = config;
            _context = context;
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] User value)
        {
            IActionResult response = Unauthorized();

            if (_context.Users.Any(user => user.Username.Equals(value.Username)))
            {
                User user = _context.Users.Where(u => u.Username.Equals(value.Username)).First();
                var client_post_hash_password = Convert.ToBase64String(Common.SaltHashPassword(
                    Encoding.ASCII.GetBytes(value.Password),
                    user.Salt));
                if (client_post_hash_password.Equals(user.Password)){
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, value.Username),
                        new Claim(ClaimTypes.Role, "User")
                    };
                    var accessToken = _tokenService.GenerateAccessToken(claims);
                    var refreshToken = _tokenService.GenerateRefreshToken();
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpireTime = DateTime.Now.AddDays(7);
                    await _context.SaveChangesAsync();
                    response = Ok(new
                    {
                        Username = user.Username,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Description = user.Description,
                        Photo = user.Photo,
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    });
                }
            }
            return response;
        }

        //private string GenerateJSONWebToken(User userInfo)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //    var permClaims = new List<Claim>();
        //    permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        //    permClaims.Add(new Claim("valid", "1"));
        //    permClaims.Add(new Claim("userid", userInfo.Username));
        //    permClaims.Add(new Claim("name", userInfo.FirstName));
        //    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        //      _config["Jwt:Issuer"],
        //      permClaims,
        //      expires: DateTime.Now.AddMinutes(120),
        //      signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }   
}
