using CRD.Interfaces;
using CRD.Repository;
using log4net;
using System.Reflection;
using System.Xml.Linq;

namespace CRD.Services
{
    public class SaleShopsService : BaseService, ISaleShopsService
    {
        protected readonly SaleShopsRepository _saleShopsRepository;

        private readonly IConfiguration configuration;

        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(SaleShopsService));
        public SaleShopsService(IConfiguration configuration, SaleShopsRepository saleShopsRepository) : base(configuration)
        {
            _saleShopsRepository = saleShopsRepository;
        }

        public async Task<SaleShopModel> CreateAsync(SaleShopRequest model)
        {
            try
            {
                using (var tw = GetTransactionWrapper())
                {
                    var insterted = await _saleShopsRepository.CreateAsync(model, tw);

                    tw.Commit();

                    return insterted;
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
 
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using (var tw = GetTransactionWrapperWithoutTransaction())
                {
                    await _saleShopsRepository.DeleteAsync(id, tw);

                }
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
        }

        public async Task<List<SaleShopModel>> GetAsync(int skip, int take, string Name, bool orderByDesc)
        {
            var tw = GetTransactionWrapperWithoutTransaction();
            var query = await _saleShopsRepository.GetSaleShopsAsync(tw);

            if (!string.IsNullOrWhiteSpace(Name))
            {
                Name = Name.ToLower();

                query = query.Where(x => x.Title.ToLower().Contains(Name)).ToList();
            }

            var total = query.Count;

            if (orderByDesc)
                query = query.OrderByDescending(x => x.Id).ToList();
            else
                query = query.OrderBy(x => x.Id).ToList();

            var data = query.Skip(skip).Take(take).ToList();

            return data;

        }

        public async Task<SaleShopModel> GetByIdAsync(int id)
        {
            var tw = GetTransactionWrapperWithoutTransaction();
            var query = await _saleShopsRepository.GetByIdAsync(id, tw);

            return query;
        }

        public async Task<SaleShopModel> UpdateAsync(SaleShopRequest model)
        {
            try
            {
                using (var tw = GetTransactionWrapper())
                {
                    var updated = await _saleShopsRepository.UpdateAsync(model, tw);

                    tw.Commit();

                    return updated;
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }

        }
    }
}
