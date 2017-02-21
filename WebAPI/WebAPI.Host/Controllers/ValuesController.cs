using JsonPatch;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI.Common.Dto.Update;
using WebAPI.Common.Dto.Write;
using WebAPI.Common.SearchParams;
using WebAPI.Contracts.BLL;
using WebAPI.Host.Filters;

namespace WebAPI.Host.Controllers
{
    [ControllerException]
    [ModelValidation]
    [Throttling]
    [Authorize]
    public class ValuesController : ApiController
    {
        private readonly IValuesFcd _valuesFcd;

        public ValuesController(IValuesFcd valuesFcd)
        {
            _valuesFcd = valuesFcd;
        }

        public async Task<IHttpActionResult> Get([FromUri]BaseSearchParams searchParams)
        {
            var items = _valuesFcd.GetAll(searchParams);

            return Ok(await items);
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            var item = _valuesFcd.Get(id);
            
            return Ok(await item);
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public async Task<IHttpActionResult> Post([FromBody]ValueWriteDto value)
        {
            var added = await _valuesFcd.Create(value);

            return CreatedAtRoute("default", new { id = added.Id }, added);
        }

        [HttpPut]
        [Authorize(Roles = "SuperUser")]
        public async Task<IHttpActionResult> Put(int id, [FromBody]ValueWriteDto value)
        {
            var updated = _valuesFcd.Update(id, value);

            return Ok(await updated);
        }

        [HttpPatch]
        [Authorize(Roles = "SuperUser")]
        public async Task<IHttpActionResult> Patch(int id, [FromBody]JsonPatchDocument<ValueUpdateDto> value)
        {
            var updated = _valuesFcd.Update(id, value);

            return Ok(await updated);
        }

        [HttpDelete]
        [Authorize(Roles = "SuperUser")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            await _valuesFcd.Delete(id);

            return Ok();
        }
    }
}
