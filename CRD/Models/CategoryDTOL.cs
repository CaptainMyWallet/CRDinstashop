namespace CRD.Models
{
    public class CategoryDTOL
    {
        public CategoryDTOL()
        {

        }
        public CategoryDTOL(Category model, string lang)
        {
            Id = model.Id;
            Title = model.Translations?.FirstOrDefault(x => x.LanguageCode == lang)?.Title;
        }
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class Category : BaseEntity
    {
        public Category()
        {
            CreateDate = DateTime.UtcNow;
            Translations = new List<CategoryTranslation>();
            AttachedEntities = new List<CategoryToAnything>();
        }
        public List<CategoryTranslation> Translations { get; set; }
        public List<CategoryToAnything> AttachedEntities { get; set; }
    }

    public class CategoryTranslation : BaseTranslation
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Title { get; set; }
    }

    public class CategoryToAnything
    {
        public int Id { get; set; }
        public int? ShopId { get; set; }
        public Shop Shop { get; set; }
        public int? WeekShopId { get; set; }
        public WeekShop WeekShop { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
