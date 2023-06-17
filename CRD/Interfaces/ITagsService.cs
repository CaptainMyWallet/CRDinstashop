using CRD.Utils;

namespace CRD.Interfaces
{
    public interface ITagsService
    {
        Task<bool> CreateAsync(TagN model);
        Task<TagDTOT> UpdateAsync(Tag model);
        Task<TagDTOT> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        List<TagToShop> GetByShopIdAsync(int id);
        Task<PaginationResponse<TagDTOT>> GetAsync(int skip, int take, string q, bool orderByDesc);
    }
}
