using BackEndBDAPP.Models;
using BackEndBDAPP.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BackEndBDAPP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[RequestRateLimit(Name = "Limit Request Code", Seconds = 1)]
    public class ForgetPasswordController : ControllerBase
    {
        private readonly UniKPContext _context = new UniKPContext();
        private readonly ITokenService _tokenService;
        public ForgetPasswordController(UniKPContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] User value)
        {
            try
            {
                if (_context.Users.Any(user => user.Username.Equals(value.Username)))
                {
                    var user = await _context.Users.FindAsync(value.Username);
                    var confirmationCode = ComfirmCodeGenerator.GenerateString(6);
                    ComfirmCode comfirmCode = new ComfirmCode()
                    {
                        Username = value.Username,
                        Code = confirmationCode
                    };
                    EmailHelper emailHelper = new EmailHelper();
                    bool emailResponse = emailHelper.SendEmail(user.Email, confirmationCode, comfirmCode.Username);
                    _context.Add(comfirmCode);
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        [HttpPost("{code}")]
        public async Task<IActionResult> PostConfirmAsync(string Code,[FromBody] User value)
        {
            var comfirmCode = _context.ComfirmCodes.Where(c => c.Username == value.Username).ToList();
            if (comfirmCode == null)
                return BadRequest("Error");
            var validCode = comfirmCode.Where(c => c.Code == Code);
            if (validCode != null)
            {
                _context.ComfirmCodes.RemoveRange(comfirmCode);
                await _context.SaveChangesAsync();
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, value.Username),
                        new Claim(ClaimTypes.Role, "ForgetPasswordUser")
                    };
                var accessToken = _tokenService.GenerateAccessToken(claims);
                return Ok(new { AccessToken = accessToken });
            }
            else
                return BadRequest("Invalid Token");

        }
        [Authorize(Roles = "ForgetPasswordUser")]
        [HttpPut]
        public async Task<IActionResult> PutPassword([FromBody] User value) {
            if (UserToken.Validate(User, value.Username))
                return Unauthorized();
            if (value.Username != value.Username)
                return BadRequest();
            var user = await _context.Users.FindAsync(value.Username);
            if (user == null)
                return NotFound();
            var salt = (Common.GetRandomSalt(16));
            user.Salt = salt;
            user.Password = Convert.ToBase64String(Common.SaltHashPassword(
                    Encoding.ASCII.GetBytes(value.Password), salt));
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
               
            }

            return Ok();
        }
    }
}
