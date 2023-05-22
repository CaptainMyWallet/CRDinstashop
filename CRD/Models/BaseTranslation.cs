using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRD.Models
{
    public class BaseTranslation
    {
        public int Id { get; set; }
        [ForeignKey("Language")]
        [StringLength(2)]
        [Required]
        public string LanguageCode { get; set; }
        public Language Language { get; set; }
    }
}
