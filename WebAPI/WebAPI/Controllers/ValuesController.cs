using JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Contracts.BLL;
using WebAPI.Model.Dto.Update;

namespace WebAPI.Controllers
{
    //[RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        private readonly IValuesFcd _valuesFcd;

        public ValuesController(IValuesFcd valuesFcd)
        {
            _valuesFcd = valuesFcd;
        }

        [Route("")]
        public IHttpActionResult Get(string sort = "id")
        {
            var items = _valuesFcd.GetAll(sort);

            return Ok(items);
        }

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            return Ok();
        }

        [HttpPatch]
        //[Route("{id}")]
        public IHttpActionResult Patch(int id, [FromBody]JsonPatchDocument<ValueUpdateDto> patch)
        //public IHttpActionResult Patch(int id, [FromBody]ValueUpdateDto patch)
        {
            //_valuesFcd.Update(id, patch);

            return Ok();
        }
    }
}
