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
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        readonly UniKPContext _context = new UniKPContext();
        readonly ITokenService _tokenService;
        public TokenController(UniKPContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }
        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(Token token)
        {
            if (token.AccessToken == null || token == null)
            {
                return BadRequest("No AccessToken");
            }
            string accessToken = token.AccessToken;
            string refreshToken = token.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpireTime <= DateTime.Now)
            {
                return BadRequest("User Problem");
            }
            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            _context.SaveChanges();
            return new ObjectResult(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }
        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            _context.SaveChanges();
            return NoContent();
        }
    }
}
