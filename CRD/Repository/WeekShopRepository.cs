using CRD.Utils;
using Dapper;

namespace CRD.Repository
{
    public class WeekShopRepository : BaseRepository
    {
        public WeekShopRepository(IConfiguration configuration) : base(configuration)
        {
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

        public async Task<PaginationResponse<WeekShop>> GetAsync(int skip, int take, string q, bool orderByDesc, TransactionWrapper tw)
        {
            var query = await tw.Connection.QueryAsync<WeekShop>(@"select * from [Instashopge].[dbo].[WeekShops]");

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.ToLower();

                query = query.Where(x => x.Title.ToLower().Contains(q));
            }

            var total = query.Count();

            if (orderByDesc)
                query = query.OrderByDescending(x => x.CreateDate);
            else
                query = query.OrderBy(x => x.CreateDate);

            var data = query.Skip(skip).Take(take).ToList();

            var response = new PaginationResponse<WeekShop>()
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


        public async Task<bool> UpdateAsync(WeekShop model, TransactionWrapper tw)
        {
            try
            {
                var affectedRows = await tw.Connection.ExecuteAsync(@"
            UPDATE [dbo].[WeekShops]
            SET [Title] = @Title, [Image] = @Image, [Link] = @Link, UpdateDate=GETDATE()
            WHERE [Id] = @Id
        ", new
                {
                    model.Id,
                    model.Title,
                    model.Image,
                    model.Link
                }, tw.Transaction);

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
