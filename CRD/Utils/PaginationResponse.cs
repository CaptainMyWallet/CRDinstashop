namespace CRD.Utils
{
    public class Pagination
    {
        public Pagination()
        {

        }
        public Pagination(int skip, int take, int count, int totalCount, string searchWord)
        {
            Skip = skip;
            Take = take;
            Count = count;
            TotalCount = totalCount;
            SearchWord = searchWord;
        }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
        public string SearchWord { get; set; }
    }
    public class PaginationResponse<T> where T : class
    {
        public PaginationResponse()
        {

        }
        public PaginationResponse(int skip, int take, int count, int totalCount, string searchWord, IEnumerable<T> data)
        {
            Meta = new Pagination(skip, take, count, totalCount, searchWord);

            Data = data;
        }
        public Pagination Meta { get; set; }
        public IEnumerable<T> Data { get; set; }

    }
}
