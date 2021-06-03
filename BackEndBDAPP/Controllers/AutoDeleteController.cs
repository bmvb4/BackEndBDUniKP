using BackEndBDAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndBDAPP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AutoDeleteController : ControllerBase
    {
        UniKPContext _context = new UniKPContext();
        public AutoDeleteController(UniKPContext context)
        {
            _context = context;
        }
        [HttpDelete("post")]
        public async void RepeatDeleteImage()
        {
            try
            {
                //List<Post> deletedPosts = _context.Posts.ToList();
                List<Post> deletedPosts = _context.Posts.Where(v => DateTime.Compare((DateTime)v.DeleteDate, DateTime.Now) < 0).ToList();
                _context.Posts.RemoveRange(deletedPosts);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }
        [HttpDelete("emailCode")]
        public async void RepeatDeleteode()
        {
            try
            {
                //List<Post> deletedPosts = _context.Posts.ToList();
                List<ComfirmCode> deletedCode = _context.ComfirmCodes.Where(v => DateTime.Compare(v.DeleteDate, DateTime.Now) < 0).ToList();
                _context.ComfirmCodes.RemoveRange(deletedCode);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
