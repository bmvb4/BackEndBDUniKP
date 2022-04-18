using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BackEndBDAPP.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BackEndBDAPP.Utils;

namespace BackEndBDAPP.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        UniKPContext _context = new UniKPContext();
        public ProfileController(UniKPContext context)
        {
            _context = context;
        }

        [HttpGet("user/get/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            if (!UserExists(id))
                return BadRequest();
            var usr = _context.Users.Select(u => new
            {
                u.Username,
                u.FirstName,
                u.LastName,
                u.Photo,
                u.Description,
                u.Posts,
                Follower = u.FollowIdFollowedNavigations.Count,
                Followed = u.FollowIdFollowerNavigations.Count,
                PostCount = u.Posts.Count,
                isFollow = _context.Follows.Any(f => (f.IdFollower == UserToken.Get(User) && f.IdFollowed == id))
            }).Where(w => w.Username == id).First();
            return Ok(usr);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDescription(string id, [FromBody] User value)
        {
            if (UserToken.Validate(User, value.Username))
                return Unauthorized();
            if (id != value.Username)
                return BadRequest();

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.Description = (value.Description != null) ? value.Description : user.Description;
            user.FirstName = (value.FirstName != null) ? value.FirstName : user.FirstName;
            user.LastName = (value.LastName != null) ? value.LastName : user.LastName;
            user.Photo = (value.Photo != null) ? value.Photo : user.Photo;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                    return NotFound();
                else
                    throw;
            }
            return Ok(user);
        }
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Username == id);
        }
    }
}
