namespace CRD.Models
{
    public class Tags
    {
        public Tags(Tag model)
        {
            Id = model.Id;
            CreateDate = model.CreateDate;
            Translations = model.Translations?.Select(x => new TagDTOT(x)).ToList();
        }
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public List<TagDTOT> Translations { get; set; }

    }
    public class TagDTOT
    {
        public TagDTOT()
        {

        }
        public TagDTOT(TagTranslation model)
        {
            Id = model.Id;
            LanguageCode = model.LanguageCode;
            Title = model.Title;
        }
        public int Id { get; set; }
        public string LanguageCode { get; set; }
        public string Title { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}