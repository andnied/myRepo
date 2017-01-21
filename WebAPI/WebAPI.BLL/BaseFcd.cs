using WebAPI.Mapper;

namespace WebAPI.BLL
{
    public abstract class BaseFcd
    {
        protected readonly WebApiMapper _mapper;

        protected BaseFcd()
        {
            _mapper = WebApiMapper.GetMapper();
        }
    }
}
