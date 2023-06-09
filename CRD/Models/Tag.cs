namespace CRD.Models
{
    public class Tag : BaseEntity
    {
        public Tag()
        {
            CreateDate = DateTime.UtcNow;
            Shops = new List<TagToShop>();
            Translations = new List<TagTranslation>();
        }
        public List<TagToShop> Shops { get; set; }
        public List<TagTranslation> Translations { get; set; }
    }
    public class TagTranslation : BaseTranslation
    {
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public string Title { get; set; }
    }

    public class TagToShop
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public int TagId { get; set; }
        public TagDTOT Tag { get; set; }
    }

    public class TagModel {
        public int Id { get; set; }
        public string Title { get; set; }

    }
}
