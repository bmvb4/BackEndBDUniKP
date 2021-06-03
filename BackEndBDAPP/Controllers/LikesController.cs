using BackEndBDAPP.Models;
using BackEndBDAPP.Utils;
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
    [Route("posts")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        UniKPContext _context = new UniKPContext();
        public LikesController(UniKPContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public Object getLikes(long id,[FromBody]string username) {
            if (UserToken.Validate(User, username))
                return Unauthorized();
            if (!PostExists(id))
                return null;
            return _context.Likes.Join(
                _context.Users,
                like => like.IdUser,
                user => user.Username,
                (like, user) => new
                {
                    IdLike = like.IdLike,
                    Username = like.IdUser,
                    UserPhoto = user.Photo,
                    IdPost = like.IdPost
                }).Where(w => w.IdPost == id).ToList();
        }

        [HttpPost("like")]
        public async Task<IActionResult> PostLike([FromBody] Like value)
        {
            if (UserToken.Validate(User, value.IdUser))
                return Unauthorized();
            if (!PostExists(value.IdPost))
                return NotFound();
            if (_context.Likes.Any(e => e.IdPost == value.IdPost && e.IdUser == value.IdUser))
                return BadRequest("1 time command");
            try
            {
                Like like = new Like()
                {
                    IdPost = value.IdPost,
                    IdUser = value.IdUser,
                    IdPostNavigation = await _context.Posts.FindAsync(value.IdPost),
                    IdUserNavigation = await _context.Users.FindAsync(value.IdUser)
                };
                like.IdPostNavigation.DeleteDate = like.IdPostNavigation.DeleteDate.Value.AddMinutes(5);
                await _context.AddAsync(like);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex + " Connection problem!");
            }
        }

        [HttpDelete("unlike")]
        public async Task<IActionResult> DeleteLike([FromBody] Like value)
        {
            if (UserToken.Validate(User, value.IdUser))
                return Unauthorized();
            if (!PostExists(value.IdPost))
                return NotFound();
            if (!_context.Likes.Any(e => e.IdPost == value.IdPost && e.IdUser == value.IdUser))
                return BadRequest("Not Liked");
            try
            {
                var unlike = _context.Likes.Where(e => e.IdPost == value.IdPost && e.IdUser == value.IdUser).First();
                unlike.IdPostNavigation = await _context.Posts.FindAsync(value.IdPost);
                unlike.IdPostNavigation.DeleteDate = unlike.IdPostNavigation.DeleteDate.Value.AddMinutes(-5);
                _context.Remove(unlike);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex + " Connection problem!");
            }
        }
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Username == id);
        }
        private bool PostExists(long id)
        {
            return _context.Posts.Any(e => e.IdPost == id);
        }
    }
}
