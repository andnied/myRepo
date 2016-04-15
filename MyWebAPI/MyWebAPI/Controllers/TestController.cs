using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyWebAPI.Controllers
{
    public class TestController : ApiController
    {
        private static List<string> Values = new List<string>
        {
            "value1",
            "value2"
        };

        public List<string> Get()
        {
            return Values;
        }

        public HttpResponseMessage Get(int id)
        {
            try
            {
                return Request.CreateResponse<string>(HttpStatusCode.OK, Values.ElementAt(id));
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item not found");
            }
        }

        public HttpResponseMessage Post([FromBody]string value)
        {
            Values.Add(value);

            var msg = Request.CreateResponse(HttpStatusCode.Created);
            msg.Headers.Location = new Uri(Request.RequestUri + (Values.Count -1).ToString());
            return msg;
        }

        public void Put(int id, [FromBody]string value)
        {
            Values[id] = value;
        }

        public void Delete(int id)
        {
            Values.RemoveAt(id);
        }
    }
}
