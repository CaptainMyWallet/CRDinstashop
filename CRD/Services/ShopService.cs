using CRD.Interfaces;
using CRD.Models;
using CRD.Repository;
using CRD.Utils;
using log4net;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRD.Services
{
    public class ShopService : BaseService, IShopsService
    {
        protected readonly ShopsRepository _shopsRepository;
        protected readonly CategoryRepository _categoryRepository;

        private readonly IConfiguration configuration;

        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(SaleShopsService));
        public ShopService(IConfiguration configuration, ShopsRepository shopsRepository,CategoryRepository categoryRepository) : base(configuration)
        {
            _shopsRepository = shopsRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ShopModel> CreateAsync(ShopRequest model)
        {
            using (var tw = GetTransactionWrapper())
            {
                var insterted = await _shopsRepository.CreateAsync(model, tw);

                tw.Commit();

                return new ShopModel
                {
                    Categories = insterted.Categories,
                    Description = insterted.Description,
                    Followers = insterted.Followers,
                    Id = insterted.Id,
                    Image = insterted.Image,
                    Link = insterted.Link,
                    Posts = insterted.Posts,
                    Tags = insterted.Tags.ToList(),
                    Title = insterted.Title
                };
            }
        }

        public Task<ShopModel> UpdateAsync(ShopRequest model)
        {
            throw new NotImplementedException();
        }

        public Task<ShopModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<Shop>> GetAsync(int skip, int take, string q, bool orderByDesc, int[] categoryIds, int[] tagIds)
        {
            using (var tw = GetTransactionWrapperWithoutTransaction())
            {
                var insterted = await _shopsRepository.GetAsync(skip, take,q, orderByDesc, categoryIds, tagIds,tw);

                return insterted;
            }
        }
    }
}
