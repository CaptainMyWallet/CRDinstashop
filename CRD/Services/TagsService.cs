using CRD.Interfaces;
using CRD.Repository;
using CRD.Utils;
using log4net;

namespace CRD.Services
{
    public class TagsService : BaseService, ITagsService
    {

        protected readonly TagsRepository _tagsRepository;

        private readonly IConfiguration configuration;

        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(SaleShopsService));
        public TagsService(IConfiguration configuration, TagsRepository tagsRepository) : base(configuration)
        {
            _tagsRepository = tagsRepository;
        }

        public async Task<bool> CreateAsync(Tag model)
        {
            using (var tw = GetTransactionWrapper())
            {
                var insterted = await _tagsRepository.CreateAsync(model, tw);

                tw.Commit();

                return insterted;
            }
        }

        public async Task<TagDTOT> UpdateAsync(Tag model)
        {
            using (var tw = GetTransactionWrapper())
            {
                var insterted = await _tagsRepository.UpdateAsync(model, tw);

                tw.Commit();

                return insterted;
            }
        }
        public List<TagToShop> GetByShopIdAsync(int id)
        {
            var tw = GetTransactionWrapperWithoutTransaction();
            var getbyID = _tagsRepository.GetByShopIdAsync(id, tw);
            return getbyID;
        }
        public async Task<TagDTOT> GetByIdAsync(int id)
        {
            var tw = GetTransactionWrapperWithoutTransaction();
            var getbyID = await _tagsRepository.GetByIdAsync(id, tw);
            return getbyID;
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            using (var tw = GetTransactionWrapper())
            {
                var res = await _tagsRepository.DeleteAsync(id, tw);
                tw.Commit();
                return res;
            }
        }

        public Task<PaginationResponse<TagDTOT>> GetAsync(int skip, int take, string q, bool orderByDesc)
        {
            var tw = GetTransactionWrapperWithoutTransaction();

            var res = _tagsRepository.GetAsync(skip, take, q, orderByDesc, tw);

            return res;
        }
    }
}
