using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
namespace BackEndBDAPP.Utils
{
    public class UserToken
    {
        public static bool Validate(ClaimsPrincipal User,string user)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    var valid = !claims.Any(p => p.Value.ToLower() == user.ToLower());
                    return valid;
                }
            }
            return false;
        }
    }
}
