using CRD.Utils;
using Dapper;

namespace CRD.Repository
{
    public class TagsRepository : BaseRepository
    {
        public TagsRepository(IConfiguration configuration) : base(configuration)
        {
        }

        //public async Task<Tag> CreateAsync(TagRequest model, TransactionWrapper tw)
        //{
        //    var insterted = await tw.Connection.QueryFirstAsync<Tag>(@"
        //                                                    declare @ID int;
        //                                                    INSERT INTO [dbo].[Tags]
        //                                                   ([Title]
        //                                                   ,[Posts]
        //                                                   ,[Followers]
        //                                                   ,[Description]
        //                                                   ,[Image]
        //                                                   ,[Link]
        //                                                   ,[CreateDate]
        //                                                   ,[UpdateDate])
        //                                             VALUES
        //                                                       (@Title,
        //                                                        @Posts,
        //                                                        @Followers,
        //                                                        @Description,
        //                                                        @Image, 
        //                                                        @Link,
        //                                                        GetDate(),
        //                                                        null
        //                                                    )
        //                                                    set @ID = SCOPE_IDENTITY();
        //                                                    select * from [dbo].[Tags] where id = @ID;
        //                                                ", new
        //    {
        //        model.Title,
        //        model.Posts,
        //        model.Followers,
        //        model.Description,
        //        model.Image,
        //        model.Link
        //    }, tw.Transaction);

        //    return insterted;
        //}
    }
}