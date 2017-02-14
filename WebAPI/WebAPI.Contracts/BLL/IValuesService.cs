using WebAPI.Common.Structures;
using WebAPI.DAL.Models;
using WebAPI.Model.SearchParams;

namespace WebAPI.Contracts.BLL
{
    public interface IValuesService
    {
        ApiCollection GetDtoCollection(ApiCollection entityCollection, BaseSearchParams searchParams);
    }
}
