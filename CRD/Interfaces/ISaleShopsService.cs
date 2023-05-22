namespace CRD.Interfaces
{
    public interface ISaleShopsService
    {

        Task<SaleShopModel> CreateAsync(SaleShopRequest model);
        Task<SaleShopModel> UpdateAsync(SaleShopRequest model);
        Task<SaleShopModel> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<List<SaleShopModel>> GetAsync(int skip, int take, string q, bool orderByDesc);
    }
}
