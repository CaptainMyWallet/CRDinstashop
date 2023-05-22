namespace CRD.Models
{
    public class ShopModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Posts { get; set; }
        public int Followers { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public List<TagToShop> Tags { get; set; }
        public List<CategoryToAnything> Categories { get; set; }
    }

    public class ShopRequest
    {
        public ShopRequest()
        {

        }
        public int Id { get; set; }
        public string Title { get; set; }
        public int Posts { get; set; }
        public int Followers { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public int[] TagIds { get; set; }
        public int[] CategoryIds { get; set; }
    }
}
