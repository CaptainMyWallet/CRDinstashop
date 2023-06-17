using CRD.Utils;

namespace CRD.Interfaces
{

    public interface IWeekShopService
    {
        Task<bool> CreateAsync(WeekShop model);
        Task<bool> DeleteAsync(int id);
        Task<PaginationResponse<querrr>> GetAsync(int skip, int take, string q, bool orderByDesc);
        Task<WeekShop> GetByIdAsync(int id);
        Task<bool> UpdateAsync(WeekShop model);
    }

}
