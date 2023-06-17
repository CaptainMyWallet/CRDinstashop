using CRD.Models;
using CRD.Utils;
using Dapper;

namespace CRD.Repository
{
    public class WeekShopRepository : BaseRepository
    {
        IConfiguration _configuration;
        public WeekShopRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> CreateAsync(WeekShop model, TransactionWrapper tw)
        {
            try
            {



                var res = await tw.Connection.QueryFirstAsync<int>(@"DECLARE @ID int;
INSERT INTO [dbo].[WeekShops]
    ([Title]
    ,[Image]
    ,[Link]
    ,[CreateDate]
    )
VALUES
    (@Title,
    @Image,
    @Link,
    GETDATE())
SET @ID = SCOPE_IDENTITY();
SELECT @ID;
 ",
                    new
                    {
                        model.Title,
                        model.Image,
                        model.Link
                    },
                    tw.Transaction
                    );
                foreach (var item in model.CategoryIds.ToList())
                {
                    await tw.Connection.QueryAsync("INSERT INTO [dbo].[CategoriesToAnything] ([WeekShopId],[CategoryId])  VALUES ( @WeekShopId,@CategoryId)", new
                    {
                        WeekShopId = res,
                        CategoryId = item,
                    }, tw.Transaction);

                }

                if (res > 0)
                {
                    return true;
                }



                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(int id, TransactionWrapper tw)
        {
            var data = await tw.Connection.ExecuteAsync("DELETE FROM [Instashopge].[dbo].[WeekShops] WHERE id = @ID",
                new { id }, tw.Transaction);
            if (data > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<PaginationResponse<querrr>> GetAsync(int skip, int take, string q, bool orderByDesc, TransactionWrapper tw)
        {
            var res = new List<querrr>();
            var query = await tw.Connection.QueryAsync<WeekShop>(@"select * from [Instashopge].[dbo].[WeekShops]");
            foreach (var item in query.ToList())
            {
                    var ss = await tw.Connection.QueryAsync<CategoryDTOL>(@"select ct.Id,ct.Title from WeekShops s with(nolock) inner join CategoriesToAnything ss with(nolock) on ss.WeekShopId=s.Id " +
                "inner join CategoryTranslation ct with(nolock) on ct.CategoryId=ss.CategoryId and ct.LanguageCode='ka' " +
                "where s.Id = @Id", new
                {
                    Id = item.Id
                }, tw.Transaction);

                res.Add(new querrr { WeekShop = item, CategoryTranslation = ss.ToList() });
            }
            
             

            var qrres = res.ToList();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.ToLower();

                qrres = qrres.Where(x => x.WeekShop.Title.ToLower().Contains(q)).ToList();
            }

            var total = qrres.Count();

            if (orderByDesc)
                qrres = qrres.OrderByDescending(x => x.WeekShop.CreateDate).ToList();
            else
                qrres = qrres.OrderBy(x => x.WeekShop.CreateDate).ToList();

            var data = qrres.Skip(skip).Take(take).ToList();

            var response = new PaginationResponse<querrr>()
            {
                Data = data,
                Meta = new Pagination() { SearchWord = q, Skip = skip, Take = take, Count = data.Count, TotalCount = total }
            };

            return response;

        }

        public async Task<WeekShop> GetByIdAsync(int id, TransactionWrapper tw)
        {

            try
            {
                var res = await tw.Connection.QueryFirstOrDefaultAsync<WeekShop>(@"select * from [Instashopge].[dbo].[WeekShops] t where t.id = @id",
                                                                                new { Id = id });
                return res;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
