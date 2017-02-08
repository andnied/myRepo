using WebAPI.Mapper;

namespace WebAPI.BLL
{
    public abstract class BaseFcd
    {
        protected readonly WebApiMapper _mapper;

        protected BaseFcd(WebApiMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
