using CRD.Utils;

namespace CRD.Interfaces
{
    public interface IShopsService
    {
        Task<ShopModel> CreateAsync(ShopRequest model);
        Task<ShopModel> UpdateAsync(ShopRequest model);
        Task<ShopModel> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<PaginationResponse<Shop>> GetAsync(int skip, int take, string q, bool orderByDesc, int[] categoryIds, int[] tagIds);

    }
}
