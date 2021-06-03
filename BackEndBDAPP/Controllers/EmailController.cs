using BackEndBDAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndBDAPP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequestRateLimit(Name = "Limit Request Number",Seconds = 5)]
    public class EmailController : ControllerBase
    {
        readonly private UniKPContext _context = new UniKPContext();
        public EmailController( UniKPContext context)
        {
            _context = context;
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
