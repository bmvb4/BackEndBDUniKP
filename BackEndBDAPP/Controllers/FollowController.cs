using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndBDAPP.Models;
using BackEndBDAPP.Utils;
using Microsoft.AspNetCore.Authorization;

namespace BackEndBDAPP.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        UniKPContext _context = new UniKPContext();
        public FollowController(UniKPContext context) {
            _context = context;
        }
        
        [HttpPost]
        public async Task<IActionResult> Follow([FromBody] Follow value)
        {
            var follow = _context.Follows.Where(f => (f.IdFollowed == value.IdFollowed && f.IdFollower == value.IdFollower)).ToList();
            if (follow.Count != 0)
                return NotFound("Have it in database");
            if (UserToken.Validate(User, value.IdFollower))
                return Unauthorized();
            if (!UserExists(value.IdFollowed))
                return NotFound("Invalid Followed");
            if (!UserExists(value.IdFollower))
                return NotFound("Invalid Follower");
            try
            {
                await _context.AddAsync(new Follow{ IdFollowed = value.IdFollowed, IdFollower=value.IdFollower });
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex + " Connection problem!");
            }
            
        }

        [HttpPost("unfollow")]
        public async Task<IActionResult> Unfollow([FromBody] Follow value)
        {
            var follow = _context.Follows.Where(f => (f.IdFollowed == value.IdFollowed && f.IdFollower == value.IdFollower)).ToList();
            if (follow.Count == 0)
                return NotFound("Dont exist");
            if (UserToken.Validate(User, follow[0].IdFollower))
                return Unauthorized();
            try
            {
                _context.Follows.Remove(follow[0]);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Username == id);
        }
    }
}
