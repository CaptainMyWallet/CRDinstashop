using CRD.Interfaces;
using CRD.Repository;
using CRD.Utils;
using log4net;
using System.Transactions;

namespace CRD.Services
{
    public class WeekShopService : BaseService, IWeekShopService
    {

        protected readonly WeekShopRepository _weekShopRepository;

        private readonly IConfiguration configuration;

        private static readonly ILog log = LogManager.GetLogger("Rolling", nameof(WeekShopService));
        public WeekShopService(IConfiguration configuration, WeekShopRepository weekShopRepository) : base(configuration)
        {
            _weekShopRepository = weekShopRepository;
        }


        public async Task<bool> CreateAsync(WeekShop model)
        {
            var tw = GetTransactionWrapperWithoutTransaction();
            var result = await _weekShopRepository.CreateAsync(model, tw);
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var tw = GetTransactionWrapper())
            {
                var result = await _weekShopRepository.DeleteAsync(id, tw);
                tw.Commit();
                return result;
            }
        }

        public async Task<WeekShop> GetByIdAsync(int id)
        {
            var tw = GetTransactionWrapperWithoutTransaction();
            var weekShop = await _weekShopRepository.GetByIdAsync(id, tw);
            return weekShop;
        }

        Task<PaginationResponse<WeekShop>> IWeekShopService.GetAsync(int skip, int take, string q, bool orderByDesc)
        {
            var tw = GetTransactionWrapperWithoutTransaction();
            var response = _weekShopRepository.GetAsync(skip, take, q, orderByDesc, tw);
            return response;
        }
        public async Task<bool> UpdateAsync(WeekShop model)
        {
            using (var tw = GetTransactionWrapper())
            {
                var result = await _weekShopRepository.UpdateAsync(model, tw);
                tw.Commit();
                return result;

            }
        }
    }
}

