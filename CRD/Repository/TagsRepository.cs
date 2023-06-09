﻿using CRD.Utils;
using Dapper;
using System.Net;

namespace CRD.Repository
{
    public class TagsRepository : BaseRepository
    {
        public TagsRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<bool> CreateAsync(TagN model, TransactionWrapper tw)
        {
            try
            {
                var InsertedId = await tw.Connection.QueryFirstAsync<int>(
                    @"DECLARE @ID INT;
            INSERT INTO [dbo].[Tags] ([CreateDate]) VALUES (GETDATE());
            SET @ID = SCOPE_IDENTITY();
            SELECT @ID;",
                    null,
                    tw.Transaction);

                if (InsertedId > 0)
                {
                    var res = await tw.Connection.QueryFirstAsync<int>(
                        @"DECLARE @ID INT;
                INSERT INTO [dbo].[TagTranslation]
                    ([TagId], [Title], [LanguageCode])
                VALUES
                    (@InsertedId, @Title, @LanguageCode);
                SET @ID = SCOPE_IDENTITY();
                SELECT @ID;",
                        new
                        {
                            InsertedId = InsertedId,
                            Title = model.Translations.First().Title,
                            LanguageCode = "ka"
                        },
                        tw.Transaction);

                    if (res > 0)
                    {
                        return true;
                    }
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
            try
            {

                var data = await tw.Connection.QueryFirstAsync<int>(@"DELETE FROM [dbo].[TagTranslation]
                                                                      WHERE TagId=@id ;
                                                                delete s from [dbo].[Tags] s
                                                                            where s.id=@id;
                                                            select Count(*) from [dbo].[Tags] where id = @id;
                                                        ", new { id }, tw.Transaction);
                if (data > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception x)
            {

                throw x.InnerException;
            }
        }

        public async Task<PaginationResponse<TagDTOT>> GetAsync(int skip, int take, string q, bool orderByDesc,TransactionWrapper tw)
        {
            var query = await tw.Connection.QueryAsync<TagDTOT>(@"select t.*,tt.Title from [Instashopge].[dbo].[Tags] t
                                                                inner join TagTranslation tt on tt.TagId=t.id");

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

            var data =  query.Skip(skip).Take(take).ToList();

            var response = new PaginationResponse<TagDTOT>()
            {
                Data = data,
                Meta = new Pagination() { SearchWord = q, Skip = skip, Take = take, Count = data.Count, TotalCount = total }
            };

            return response;

        }

        public async Task<TagDTOT> GetByIdAsync(int id, TransactionWrapper tw)
        {

            try
            {
                var res = await tw.Connection.QueryFirstOrDefaultAsync<TagDTOT>(@"select tt.TagId as id, tt.Title from[Instashopge].[dbo].[Tags] t
                                                                            inner join TagTranslation tt on tt.TagId = t.id
                                                                               where t.id = @id and tt.LanguageCode = 'ka'",
                                                                                new { Id = id });
                return res;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TagToShop> GetByShopIdAsync(int ShopId, TransactionWrapper tw)
        {

            try
            {
                var res = tw.Connection.Query<TagToShop>(@"SELECT *
                                                                                 FROM [Instashopge].[dbo].[TagsToShops] s where s.ShopId=@id ",
                                                                                new { Id = ShopId }).ToList();

                return res;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TagDTOT> UpdateAsync(Tag model, TransactionWrapper tw)
        {
            try
            {

                var data = await tw.Connection.QueryFirstOrDefaultAsync<TagDTOT>(@"update tt set Title=@Title from [Instashopge].[dbo].[Tags] t
                                                                                inner join TagTranslation tt on tt.TagId=t.id       
                                                                               where t.id = @id and tt.LanguageCode = 'ka'",
                                                                                    new
                                                                                    {
                                                                                        id = model.Id,
                                                                                        UpdateDate = DateTime.Now,
                                                                                        Title = model.Translations.First().Title,
                                                                                    }, tw.Transaction);

                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}