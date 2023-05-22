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
            try
            {
                var query = await tw.Connection.QueryAsync<Shop>(@"select * from [Instashopge].[dbo].[Shops]");
                if (!string.IsNullOrWhiteSpace(q))
                {
                    q = q.ToLower();

                    query = query.Where(x => x.Title.ToLower().Contains(q) || x.Description.ToLower().Contains(q));
                }

                if (tagIds != null && tagIds.Length != 0)
                {
                    var TagsToShops = await tw.Connection.QueryAsync<int>(@"select s.ShopId from [Instashopge].[dbo].[TagsToShops] s where s.TagId in(" + String.Join(",", tagIds) + ")");

                    query = query.Where(x => TagsToShops.Contains(x.Id));
                }

                if (categoryIds != null && categoryIds.Length != 0)
                {
                    var Categories = await tw.Connection.QueryAsync<int>(@"select s.ShopId from [Instashopge].[dbo].[CategoriesToAnything] s where s.CategoryId in(" + String.Join(",", categoryIds) + ")");

                    query = query.Where(x => Categories.Contains(x.Id));
                }

                var total = query.Count();

                if (orderByDesc)
                    query = query.OrderByDescending(x => x.CreateDate);
                else
                    query = query.OrderBy(x => x.CreateDate);

                var data = query.Skip(skip).Take(take).ToList();

                var response = new PaginationResponse<Shop>()
                {
                    Data = data,
                    Meta = new Pagination() { SearchWord = q, Skip = skip, Take = take, Count = data.ToList().Count, TotalCount = total }
                };

                return response;
            }catch(Exception x)
            {
                throw x;
            }
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
