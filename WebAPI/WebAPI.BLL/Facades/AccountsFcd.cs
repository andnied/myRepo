using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.BLL.Services;
using WebAPI.Common.Dto.Write;
using WebAPI.Contracts.BLL;
using WebAPI.Mapper;
using WebAPI.Model.Models.Identity;

namespace WebAPI.BLL.Facades
{
    public class AccountsFcd : BaseFcd, IAccountsFcd
    {
        private readonly ApplicationUserManager _userManager;

        public AccountsFcd(ApplicationUserManager userManager, WebApiMapper mapper)
            : base(mapper)
        {
            _userManager = userManager;
        }

        public async Task<string> Create(ApplicationUserWriteDto model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            
            return user.Id;
        }
    }
}
