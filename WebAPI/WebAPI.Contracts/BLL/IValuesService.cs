using WebAPI.Common.Structures;
using WebAPI.Model.Models;
using WebAPI.Common.SearchParams;

namespace WebAPI.Contracts.BLL
{
    public interface IValuesService
    {
        ApiCollection GetDtoCollection(ApiCollection entityCollection, BaseSearchParams searchParams);
    }
}
