using BackEndBDAPP.Models;
using BackEndBDAPP.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndBDAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        readonly private UniKPContext _context = new UniKPContext();
        public EmailController( UniKPContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Generate([FromBody] User user) {
            if (user.EmailConfirm==true)
            {
                return BadRequest("");
            }
            var confirmationCode = ComfirmCodeGenerator.GenerateString(6);
            EmailHelper emailHelper = new EmailHelper();
            bool emailResponse = emailHelper.SendEmail(user.Email, confirmationCode, user.Username);
            if (!emailResponse)
                return BadRequest("Email Problem!");
            return Ok(emailResponse);
            try
            {
                ComfirmCode comfirmCode = new ComfirmCode()
                {
                    Username = user.Username,
                    Code = confirmationCode
                };
                _context.Add(comfirmCode);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Save problem!");
            }
        }
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string Username, string Code)
        {
            var comfirmCode = _context.ComfirmCodes.Where(c => c.Username == Username).ToList();
            if (comfirmCode == null)
                return BadRequest("Error");
            var validCode = comfirmCode.Where(c => c.Code == Code);
            if (validCode != null)
            {
                var user = await _context.Users.FindAsync(Username);
                user.EmailConfirm = true;
                _context.ComfirmCodes.RemoveRange(comfirmCode);
                await _context.SaveChangesAsync();
                return Ok();
            }else
                return BadRequest("Invalid Token");

        }

    }
}
