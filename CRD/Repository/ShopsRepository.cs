using CRD.Utils;
using Dapper;

namespace CRD.Repository
{
    public class ShopsRepository : BaseRepository
    {
        public ShopsRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Shop> CreateAsync(ShopRequest model, TransactionWrapper tw)
        {
            var insterted = await tw.Connection.QueryFirstAsync<Shop>(@"
                                                            declare @ID int;
                                                            INSERT INTO [dbo].[Shops]
                                                           ([Title]
                                                           ,[Posts]
                                                           ,[Followers]
                                                           ,[Description]
                                                           ,[Image]
                                                           ,[Link]
                                                           ,[CreateDate]
                                                           ,[UpdateDate])
                                                     VALUES
                                                               (@Title,
                                                                @Posts,
                                                                @Followers,
                                                                @Description,
                                                                @Image, 
                                                                @Link,
                                                                GetDate(),
                                                                null
                                                            )
                                                            set @ID = SCOPE_IDENTITY();
                                                            select * from [dbo].[Shops] where id = @ID;
                                                        ", new
            {
                model.Title,
                model.Posts,
                model.Followers,
                model.Description,
                model.Image,
                model.Link
            }, tw.Transaction);

            return insterted;
        }

        public async Task DeleteAsync(int id, TransactionWrapper tw)
        {
            await tw.Connection.ExecuteAsync(@"delete from [Instashopge].[dbo].[Shops] 
                                                            where id = @Id",
                                                            new
                                                            {
                                                                Id = id
                                                            });
            tw.Commit();
        }

        public async Task<PaginationResponse<Shop>> GetAsync(int skip, int take, string q, bool orderByDesc, int[] categoryIds, int[] tagIds, TransactionWrapper tw)
        {
            var quer = "select * from [Instashopge].[dbo].[Shops] s where 1=1 ";
            if (categoryIds.Length > 0)
            {
                quer += "AND s.";
            }
            var ShopList = await tw.Connection.QueryAsync<Shop>(@"select * from [Instashopge].[dbo].[Shops]");
             
            var response = new PaginationResponse<Shop>()
            {
                Data = ShopList.ToList(),
                Meta = new Pagination() { SearchWord = q, Skip = skip, Take = take, Count = ShopList.ToList().Count, TotalCount = 1 }
            };

            return response;
        }

        public async Task<Shop> GetByIdAsync(int id)
        {
            //var data = await _db.Shops
            //    .Include(x => x.Categories)
            //    .ThenInclude(x => x.Category)
            //    .ThenInclude(x => x.Translations)
            //    .Include(x => x.Tags)
            //    .ThenInclude(x => x.Tag)
            //    .ThenInclude(x => x.Translations)
            //    .FirstOrDefaultAsync(x => x.Id == id);

            //return new Shop(data, _lang);
            throw new NotImplementedException();
        }

        public async Task<Shop> UpdateAsync(ShopRequest model)
        {
            //var data = await _db.Shops.Include(x => x.Categories).Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == model.Id);

            //ShopTransformer.Transform(data, model);

            //await _db.SaveChangesAsync();

            //data = await _db.Shops
            //    .Include(x => x.Categories)
            //    .ThenInclude(x => x.Category)
            //    .ThenInclude(x => x.Translations)
            //    .Include(x => x.Tags)
            //    .ThenInclude(x => x.Tag)
            //    .ThenInclude(x => x.Translations)
            //    .FirstOrDefaultAsync(x => x.Id == model.Id);

            //return new Shop(data, _lang);

            throw new NotImplementedException();
        }

    }
}
