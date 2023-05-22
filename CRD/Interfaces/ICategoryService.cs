namespace CRD.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTOL>> GetCategory();
    }
}
