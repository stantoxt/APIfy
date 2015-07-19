using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace APIfy.Example.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            IEnumerable<string> authHeader;
            if (!actionContext.Request.Headers.TryGetValues("API-AUTH", out authHeader))
            {
                var response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                response.Content = new StringContent("Invalid auth token found. Unauthorized action.");
                actionContext.Response = response;
            }
            else
            {
                // TO-DO : CHECK USING AUTH LOGIC
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            
        }
    }
}