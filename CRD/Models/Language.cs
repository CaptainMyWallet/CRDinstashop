using System.ComponentModel.DataAnnotations;

namespace CRD.Models
{
    public class Language
    {
        [Required]
        [StringLength(2)]
        public string Code { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
