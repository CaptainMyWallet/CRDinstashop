using CRD.Models;
using CRD.Utils;
using Dapper;

namespace CRD.Repository
{
    public class SaleShopsRepository : BaseRepository
    {
        public SaleShopsRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<List<SaleShopModel>> GetSaleShopsAsync(TransactionWrapper tw)
        {
            var category = await tw.Connection.QueryAsync<SaleShopModel>(@"select * from [Instashopge].[dbo].[SaleShops]");

            return category.ToList();
        }
        public async Task<SaleShopModel> GetByIdAsync(int id, TransactionWrapper tw)
        {
            var category = await tw.Connection.QueryFirstOrDefaultAsync<SaleShopModel>(@"select * from [Instashopge].[dbo].[SaleShops] 
                                                            where id = @id", new { Id = id });

            return category;
        }
        public async Task<SaleShopModel> CreateAsync(SaleShopRequest model, TransactionWrapper tw)
        {
            var insterted = await tw.Connection.QueryFirstAsync<SaleShopModel>(@"
                                                            declare @ID int;
                                                            INSERT INTO [dbo].[SaleShops]
                                                           ([Title]
                                                           ,[Image]
                                                           ,[Link]
                                                           ,[CreateDate]
                                                           ,[UpdateDate])
                                                        VALUES
                                                            (@Title,
                                                                @Image,
                                                                @Link,
                                                                GetDate(),
                                                                null
                                                            )
                                                            set @ID = SCOPE_IDENTITY();
                                                            select * from [dbo].[SaleShops] where id = @ID;
                                                        ", new
            {

                model.Title,
                model.Image,
                model.Link,

            }, tw.Transaction);

            return insterted;

        }

        public async Task<SaleShopModel> UpdateAsync(SaleShopRequest model, TransactionWrapper tw)
        {
            var updated = await tw.Connection.QueryFirstAsync<SaleShopModel>(@"UPDATE [dbo].[SaleShops]
                                                SET [Title] = @Title
                                                  ,[Image] = @Image
                                                  ,[Link] = @Link
                                                  ,[UpdateDate] = GetDate()
                                             WHERE ID = @id;
                                             select * from [dbo].[SaleShops] where id = @id;
                                            ",
        new
        {
            model.Title,
            model.Image,
            model.Link,
            @id = model.Id
        }, transaction: tw.Transaction);
            return updated;
        }

        public async Task DeleteAsync(int id, TransactionWrapper tw)
        {
            var deleted = await tw.Connection.ExecuteAsync(@"delete from [Instashopge].[dbo].[SaleShops] 
                                                            where id = @Id",
                                                            new
                                                            {
                                                                Id = id
                                                            });
            tw.Commit();


        }
    }
}
