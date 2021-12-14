using Microsoft.AspNetCore.Http;
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
    [Route("posts")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        UniKPContext _context = new UniKPContext();
        public CommentsController(UniKPContext context) {
            _context = context;
        }
        [HttpGet("comment/get/{id}/{page}")]
        public Object GetComments(long id, int page)
        {

            if (!PostExists(id))
                return null;
            var rez = _context.Comments.Join(
                _context.Users,
                comment => comment.IdUser,
                user => user.Username,
                (comment, user) => new
                {
                    IdComment = comment.IdComment,
                    CommentText = comment.CommentText,
                    IdUser = comment.IdUser,
                    UserPhoto = user.Photo,
                    IdPost = comment.IdPost
                }).Where(w => w.IdPost == id).Skip(10 * page).Take(10).ToList();
            return rez;
        }

        //[HttpGet("comment/get/{id}/{page}")]
        //public Object GetComments(long id, int page)
        //{

        //    if (!PostExists(id))
        //        return null;
        //    var rez = _context.Comments.Join(
        //        _context.Users,
        //        comment => comment.IdUser,
        //        user => UserToken.Get(User),
        //        (comment, user) => new
        //        {
        //            IdComment = comment.IdComment,
        //            CommentText = comment.CommentText,
        //            IdUser = comment.IdUser,
        //            UserPhoto = user.Photo,
        //            IdPost = comment.IdPost
        //        }).Where(w => w.IdPost == id).Skip(10 * page).Take(10).ToList();
        //    return rez;
        //}

        [HttpPost("comment")]
        public async Task<IActionResult> PostComment([FromBody] Comment value)
        {
            if (UserToken.Validate(User, value.IdUser))
                return Unauthorized();
            if (!PostExists(value.IdPost))
                return NotFound();
            try
            {
                Comment commnet = new Comment()
                {
                    IdPost = value.IdPost,
                    IdUser = value.IdUser,
                    CommentText = value.CommentText
                };
                await _context.AddAsync(commnet);
                await _context.SaveChangesAsync();
                return Ok(commnet.IdComment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex + " Connection problem!");
            }
        }

        [HttpDelete("comment")]
        public async Task<IActionResult> DeleteComment([FromBody] Comment value)
        {
            if (UserToken.Validate(User, value.IdUser))
                return Unauthorized();
            if (!PostExists(value.IdPost))
                return NotFound();
            try
            {
                var deleteComment = await _context.Comments.FindAsync(value.IdComment);
                _context.Comments.Remove(deleteComment);
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
