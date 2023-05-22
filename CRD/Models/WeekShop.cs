namespace CRD.Models
{
    public class WeekShop : BaseEntity
    {
        public WeekShop()
        {
            CreateDate = DateTime.UtcNow;
            Categories = new List<CategoryToAnything>();
        }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public List<CategoryToAnything> Categories { get; set; }
    }
}
