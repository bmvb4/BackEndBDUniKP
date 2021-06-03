
using BackEndBDAPP.Models;
using BackEndBDAPP.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace BackEndBDAPP.Controllers
{   
    [Route("[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UniKPContext _context = new UniKPContext();
        private readonly ITokenService _tokenService;
        public RegisterController(IConfiguration config, UniKPContext context, ITokenService tokenService)
        {
            _config = config;
            _context = context;
            _tokenService = tokenService;
        }
        [HttpPost]
        public IActionResult Post([FromBody]User value)
        {
            if (!_context.Users.Any(user => user.Username.Equals(value.Username) )) {

                var salt = (Common.GetRandomSalt(16));
                User user = new User() {
                    Username = value.Username,
                    Salt = salt,
                    Password = Convert.ToBase64String(Common.SaltHashPassword(
                    Encoding.ASCII.GetBytes(value.Password), salt)),
                    FirstName = value.FirstName,
                    LastName = value.LastName,
                    Email = value.Email,
                    Photo = Convert.FromBase64String(DefautImage.image)
            };
                
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, value.Username),
                        new Claim(ClaimTypes.Role, "User")
                    };
                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpireTime = DateTime.Now.AddDays(20);
                var confirmationCode = ComfirmCodeGenerator.GenerateString(6);
                EmailHelper emailHelper = new EmailHelper();
                bool emailResponse = emailHelper.SendEmail(user.Email, confirmationCode);
                if (!emailResponse)
                    return BadRequest("Email Problem!");
                try
                {
                    _context.Add(user);
                    _context.SaveChanges();
                    ComfirmCode comfirmCode = new ComfirmCode()
                    {
                        Username = user.Username,
                        Code = confirmationCode
                    };
                    _context.Add(comfirmCode);
                    _context.SaveChanges();
                    return Ok(new
                    {
                        Username = user.Username,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Description = user.Description,
                        Photo = user.Photo,
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest("Save problem!");
                }
            }
            else
            {
                return BadRequest("User is existing in Database!!!");
            }
        }
    }
}
