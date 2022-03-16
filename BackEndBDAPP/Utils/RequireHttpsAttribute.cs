using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BackEndBDAPP.Utils
{
    public class RequireHttpsAttribute: AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            HttpRequestMessage request = actionContext.Request;
            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                if (request.Method.Equals(HttpMethod.Get))
                {
                    actionContext.Response = request.CreateResponse(HttpStatusCode.Found, "SSL is required");

                    // Provide the correct URL to the user via the Location header.
                    var uriBuilder = new UriBuilder(request.RequestUri)
                    {
                        Scheme = Uri.UriSchemeHttps,
                        Port = 443
                    };

                    actionContext.Response.Headers.Location = uriBuilder.Uri;
                }
                else actionContext.Response = request.CreateResponse(HttpStatusCode.NotFound, "SSL is required");
            }
            
        }
    }
}
