﻿using CRD.Interfaces;
using CRD.Utils;
using Dapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRD.Repository
{
    public class ShopsRepository : BaseRepository
    {
        ITagsService _TagsService;
        public ShopsRepository(IConfiguration configuration, ITagsService tagsService) : base(configuration)
        {
            _TagsService = tagsService;
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

            try
            {
                if (model.TagIds != null && model.TagIds.Length > 0)
                {
                    foreach (int tagId in model.TagIds)
                    {
                        await tw.Connection.ExecuteAsync(@"
                INSERT INTO [Instashopge].[dbo].[TagsToShops] ([ShopId], [TagId])
                VALUES (@ShopId, @TagId)
            ", new
                        {
                            ShopId = insterted.Id,
                            TagId = tagId
                        }, tw.Transaction);
                    }
                }

            }
            catch (Exception e)
            {
                
            }
           


            return insterted;
        }

        public async Task<bool> DeleteAsync(int id, TransactionWrapper tw)
        {
            var data = await tw.Connection.ExecuteAsync(@"delete s from [Instashopge].[dbo].[Shops] s
                                                            where Id = @Id",
                                                            new
                                                            {
                                                                Id = id
                                                            }, tw.Transaction);
            if (data > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
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
                    var TagsToShops = await tw.Connection.QueryAsync<int>(@"select s.ShopId from [Instashopge].[dbo].[TagsToShops] s where s.TagId in(" + string.Join(",", tagIds) + ")");

                    query = query.Where(x => TagsToShops.Contains(x.Id));
                }

                if (categoryIds != null && categoryIds.Length != 0)
                {
                    var Categories = await tw.Connection.QueryAsync<int?>(@"select s.ShopId from [Instashopge].[dbo].[CategoriesToAnything] s where s.CategoryId in(" + string.Join(",", categoryIds) + ")");

                    query = query.Where(x => Categories.Contains(x.Id));
                }

                foreach (var item in query)
                {
                    item.Tags = _TagsService.GetByShopIdAsync(item.Id);
                    foreach (var item2 in item.Tags)
                    {
                        item2.Tag = await _TagsService.GetByIdAsync(item2.TagId);
                    }
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
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public async Task<Shop> GetByIdAsync(int id, TransactionWrapper tw)
        {
            var shop = await tw.Connection.QueryFirstOrDefaultAsync<Shop>(@"select * from [Instashopge].[dbo].[Shops] 
                                                            where id = @id", new { Id = id });

            return shop;
        }

        public async Task<Shop> UpdateAsync(ShopRequest model, TransactionWrapper tw)
        {
            var updated = await tw.Connection.QueryFirstAsync<Shop>(@"UPDATE [dbo].[Shops]
                                                                   SET [Title] = @Title
                                                                      ,[Posts] = @Posts
                                                                      ,[Followers] = @Followers
                                                                      ,[Description] = @Description
                                                                      ,[Image] = @Image
                                                                      ,[Link] = @Link
                                                                      ,[UpdateDate] = Getdate()

                                             WHERE ID = @id;
                                             select * from [dbo].[Shops] where id = @id;
                                            ",
        new
        {
            model.Title,
            model.Posts,
            model.Followers,
            model.Description,
            model.Image,
            model.Link,
            @id = model.Id
        }, transaction: tw.Transaction);
            return updated;
        }
        public async Task AddCountByShopId(int Id, TransactionWrapper tw)
        {
            try
            {
                await tw.Connection.QueryFirstAsync<int>(@"UPDATE [dbo].[Shops]
                                                                   SET 
                                                                        Count = Count + 1
                                                                      ,[UpdateDate] = Getdate()
																	  where id= @id;
                                            select 2",
                                                                new
                                                                {
                                                                    @id = Id
                                                                }, transaction: tw.Transaction);
            }
            catch (Exception x)
            {

                throw x;
            }
            

        }
    }
}
