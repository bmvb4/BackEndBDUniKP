using BackEndBDAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndBDAPP.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        readonly UniKPContext _context = new UniKPContext();
        public SearchController(UniKPContext context)
        {
            _context = context;
        }

        [HttpPost("user/{search}")]
        public async Task<IActionResult> User(string search) {
            if (string.IsNullOrEmpty(search) || string.IsNullOrWhiteSpace(search))
                return BadRequest("Invalid Search!");
            else {
                var users = _context.Users.Where(u => u.Username.ToLower().Contains(search.ToLower())).Select(u => new { u.Username, u.Photo }).Take(10).ToList();
                return Ok(users);
            }
        }
        [HttpPost("tag/{search}")]
        public async Task<IActionResult> Tag(string search)
        {
            if (string.IsNullOrEmpty(search) || string.IsNullOrWhiteSpace(search))
                return BadRequest("Invalid Search!");
            else
            {
                var tags = _context.Tags.Where(t => t.TagName.ToLower().Contains(search.ToLower())).Select(t => t.TagName).Take(10).ToList();
                return Ok(tags);
            }
        }
    }
}
