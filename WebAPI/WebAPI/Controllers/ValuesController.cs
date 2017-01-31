﻿using JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI.Contracts.BLL;
using WebAPI.Filters;
using WebAPI.Model.Dto.Read;
using WebAPI.Model.Dto.Update;
using WebAPI.Model.Dto.Write;
using WebAPI.Model.SearchParams;

namespace WebAPI.Controllers
{
    [WebApiException]
    [ModelValidation]
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
            var item = await _valuesFcd.Get(id);

            if (item != null)
                return Ok(item);
            else
                return NotFound();
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]ValueWriteDto value)
        {
            var added = await _valuesFcd.Create(value);

            return CreatedAtRoute("DefaultApi", new { id = added.Id }, added);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, [FromBody]ValueWriteDto value)
        {
            var updated = _valuesFcd.Update(id, value);

            return Ok(await updated);
        }

        [HttpPatch]
        public async Task<IHttpActionResult> Patch(int id, [FromBody]JsonPatchDocument<ValueUpdateDto> value)
        {
            var updated = _valuesFcd.Update(id, value);

            return Ok(await updated);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            await _valuesFcd.Delete(id);

            return Ok();
        }
    }
}