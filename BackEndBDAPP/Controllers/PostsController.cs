using BackEndBDAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BackEndBDAPP.Utils;
using Microsoft.EntityFrameworkCore;

namespace BackEndBDAPP.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        UniKPContext _context = new UniKPContext();
        public PostsController(UniKPContext context)
        {
            _context = context;
        }

        [HttpGet("getAll/{id}/{page}")]
        public async Task<Object> GetUserImages(string id,int page,[FromBody]User username)
        {
            if (!UserExists(id))
                return null;
            return _context.Posts.Join(
                _context.Users,
                post => post.IdUser,
                user => user.Username,
                (post, user) => new
                {
                    IdPost = post.IdPost,
                    Photo = post.Photo,
                    Description = post.Description,
                    CreateDate = post.CreateDate,
                    DeleteDate = post.DeleteDate,
                    IdUser = post.IdUser,
                    UserPhoto = user.Photo,
                    LikesCounter = post.Likes.Count,
                    CommentsCounter = post.Comments.Count,
                    isFollow = _context.Follows.Any(f => (f.IdFollower == username.Username && f.IdFollowed == user.Username)),
                    isLiked = _context.Likes.Any(l => l.IdUser == username.Username && l.IdPost == post.IdPost),
                    Tags = _context.Tags1.Where(t => t.IdPost == post.IdPost).Select(t => t.IdTag).ToList()
                }).Where(w => w.IdUser == id).Skip(10*page).Take(10).ToList();
        }

        [HttpGet("getLast/{page}")]
        public async Task<Object> GetLastImages(int page,[FromBody] User username)
        {
          
            if (!UserExists(username.Username))
                return null;
            return _context.Posts.Join(
                _context.Users,
                post => post.IdUser,
                user => user.Username,
                (post, user) => new
                {
                    IdPost = post.IdPost,
                    Photo = post.Photo,
                    Description = post.Description,
                    CreateDate = post.CreateDate,
                    DeleteDate = post.DeleteDate,
                    IdUser = post.IdUser,
                    UserPhoto = user.Photo,
                    LikesCounter = post.Likes.Count,
                    CommentsCounter = post.Comments.Count,
                    isFollow = _context.Follows.Any(f => (f.IdFollower == username.Username && f.IdFollowed == user.Username)),
                    isLiked = _context.Likes.Any(l => l.IdUser == username.Username && l.IdPost == post.IdPost),
                    Tags = _context.Tags1.Where(t => t.IdPost == post.IdPost).Select(t => t.IdTag).ToList()
                }).OrderByDescending(d => d.DeleteDate).Skip(10 * page).Take(10).ToList();
        }

        [HttpGet("get/{id}")]
        public IActionResult GetImageAsync(long id, User username)
        {
            try
            {
                var img = _context.Posts.Join(
                _context.Users,
                post => post.IdUser,
                user => user.Username,
                (post, user) => new
                {
                    idPost = post.IdPost,
                    Photo = post.Photo,
                    Description = post.Description,
                    CreateDate = post.CreateDate,
                    DeleteDate = post.DeleteDate,
                    Username = post.IdUser,
                    UserPhoto = user.Photo,
                    LikesCounter = _context.Likes.Count(l => l.IdPost == post.IdPost),
                    CommentsCounter = _context.Comments.Count(c => c.IdPost == post.IdPost),
                    isFollow = _context.Follows.Any(f => (f.IdFollower == username.Username && f.IdFollowed == user.Username)),
                    isLiked = _context.Likes.Any(l => l.IdUser == username.Username && l.IdPost == post.IdPost),
                    Tags = _context.Tags1.Where(t => t.IdPost == post.IdPost).Select(t => t.IdTag).ToList()
                }).Where(w => w.idPost == id).First();
                return Ok(img);
            }
            catch (Exception)
            {
                return BadRequest();
            }


            
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutPost(long id, [FromBody] Post value)
        {
            if (UserToken.Validate(User, value.IdUser))
                return Unauthorized();
            if (id != value.IdPost)
                return BadRequest("Invalid Request!");

            try
            {
                var post = await _context.Posts.FindAsync(id);
                if (post == null)
                    return NotFound();

                post.Description = value.Description;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            return Ok("updated");
        }

        [HttpPost("post")]
        public async Task<IActionResult> PostImage([FromBody] PostView value)
        {
            if (UserToken.Validate(User, value.IdUser))
                return Unauthorized();
            if (value.Photo == null)
                return BadRequest("No Photo");
            if (!UserExists(value.IdUser))
                return NotFound();
            try
            {
                
                Post post = new Post()
                {
                    Photo = value.Photo,
                    IdUser = value.IdUser,
                    CreateDate = DateTime.Now,
                    DeleteDate = DateTime.Now.AddDays(1),
                    Description = value.Description
                };
                await _context.AddAsync(post);
                await _context.SaveChangesAsync();
                if (value.tags != null)
                    foreach (var tag in value.tags)
                    {
                        if (!_context.Tags.Any(t => t.TagName == tag))
                            await _context.AddAsync(new Tag{TagName = tag});
                        Tag1 tagPost = new Tag1()
                        {
                            IdPost = post.IdPost,
                            IdTag = tag
                        };
                        await _context.AddAsync(tagPost);
                    }
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex + " Connection problem!");
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteImage([FromBody] Post value)
        {
            if (UserToken.Validate(User, value.IdUser))
                return Unauthorized();
            if (!PostExists(value.IdPost))
                return BadRequest("Invalid Request!");
            try
            {
                var imageToDelete = await _context.Posts.FindAsync(value.IdPost);
                if (imageToDelete == null)
                {
                    return NotFound($"Image with Id = {value.IdPost} not found");
                }

                _context.Posts.Remove(imageToDelete);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
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
