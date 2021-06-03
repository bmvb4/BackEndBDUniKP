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
    public class TagController : ControllerBase
    {
        UniKPContext _context = new UniKPContext();
        public TagController(UniKPContext context)
        {
            _context = context;
        }

        [HttpPost("{id}")]
        public IActionResult Post(long id,[FromBody] List<Tag> value)
        {
            return Ok();  
        }
    }
}
