namespace CRD.Models
{
    public class Shop : BaseEntity
    {
        public Shop()
        {
            CreateDate = DateTime.UtcNow;
            Tags = new List<TagToShop>();
            Categories = new List<CategoryToAnything>();
        }
        public string Title { get; set; }
        public int Posts { get; set; }
        public int Followers { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public List<TagToShop> Tags { get; set; }
        public List<CategoryToAnything> Categories { get; set; }
    }
}
