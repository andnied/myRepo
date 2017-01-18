using JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Contracts.BLL;
using WebAPI.Model.Dto.Update;
using WebAPI.Model.SearchParams;

namespace WebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly IValuesFcd _valuesFcd;

        public ValuesController(IValuesFcd valuesFcd)
        {
            _valuesFcd = valuesFcd;
        }
        
        public IHttpActionResult Get([FromUri]BaseSearchParams searchParams)
        {
            var items = _valuesFcd.GetAll(searchParams);

            return Ok(items);
        }
        
        public IHttpActionResult Get(int id)
        {
            var item = _valuesFcd.Get(id);

            if (item != null)
                return Ok(item);
            else
                return NotFound();
        }

        [HttpPatch]
        public IHttpActionResult Patch(int id, [FromBody]JsonPatchDocument<ValueUpdateDto> patch)
        {
            var updated = _valuesFcd.Update(id, patch);

            return Ok(updated);
        }
    }
}
