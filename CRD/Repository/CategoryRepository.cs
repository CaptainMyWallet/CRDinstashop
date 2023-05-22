using CRD.Utils;
using Dapper;

namespace CRD.Repository
{
    public class CategoryRepository : BaseRepository
    {
        public CategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List< Category>> GetCategory(TransactionWrapper tw)
        {
            var category = await tw.Connection.QueryAsync<Category>(@"select * from [Instashopge].[dbo].[Categories]");

            return category.ToList();
        }
        
        public async Task<List<CategoryTranslation>> GetCategoryTranslation(TransactionWrapper tw)
        {
            var categoryTranslation = await tw.Connection.QueryAsync<CategoryTranslation>(@"select * from [Instashopge].[dbo].[CategoryTranslation]");

            return categoryTranslation.ToList();
        }
    }
}
