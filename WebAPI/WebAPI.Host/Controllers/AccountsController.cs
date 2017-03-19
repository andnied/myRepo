using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebAPI.Common.Dto.Write;
using WebAPI.Contracts.BLL;
using WebAPI.Host.Filters;

namespace WebAPI.Host.Controllers
{
    [ControllerException]
    [ModelValidation]
    public class AccountsController : ApiController
    {
        private readonly IAccountsFcd _accountsFcd;

        public AccountsController(IAccountsFcd fcd)
        {
            _accountsFcd = fcd;
        }
        
        public async Task<IHttpActionResult> Post([FromBody]ApplicationUserWriteDto model)
        {
            var id = await _accountsFcd.Create(model);

            return Ok();
        }
    }
}