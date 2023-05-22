using CRD.Interfaces;
using CRD.Repository;
using log4net;

namespace CRD.Services
{
    public class CategoryService:BaseService, ICategoryService
    {
        protected readonly CategoryRepository _categoryRepository;
        private readonly IConfiguration configuration;

        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(LoanService));
        public CategoryService(IConfiguration configuration, CategoryRepository categoryRepository) : base(configuration)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryDTOL>> GetCategory()
        {
            var tw = GetTransactionWrapperWithoutTransaction();
            var ct = await GetCategoryTranslation();
            var result = await _categoryRepository.GetCategory(tw);

            var query = (from a in result
                         join b in ct on a.Id equals b.CategoryId
                         where b.LanguageCode == "ka"
                         select new CategoryDTOL { Id = a.Id, Title = b.Title }).ToList();

            return query;
        }

        public async Task<List<CategoryTranslation>> GetCategoryTranslation()
        {
            var tw = GetTransactionWrapperWithoutTransaction();
            var result = await _categoryRepository.GetCategoryTranslation(tw);

            return result;
        }
    }
}
