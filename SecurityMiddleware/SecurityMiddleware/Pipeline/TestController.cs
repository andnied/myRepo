using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;

namespace SecurityMiddleware.Pipeline
{
    [TestAuthenticationFilter]
    [TestAuthorizationFilter]
    public class TestController : ApiController
    {
        public IHttpActionResult Get()
        {
            Helper.Write("Controller", User);
            //Helper.Write("Controller by Net.RequestContext", RequestContext.Principal);
            //Helper.Write("Controller by RequestContext", Request.GetRequestContext().Principal);

            return Ok();
        }
    }
}