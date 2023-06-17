namespace CRD.Models
{
    public class CategoriesToAnything
    {
        public int Id { get; set; }
        public int? ShopId { get; set; }
        public int? WeekShopId { get; set; }
        public int CategoryId { get; set; }

    }

    public class querrr
    {
        public WeekShop WeekShop { get; set; }
        public List<CategoryDTOL> CategoryTranslation { get; set; }
    }
}
