using System.ComponentModel.DataAnnotations;

namespace internetprogramciligi1.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [Display(Name = "Kategori Adı")]
        public string Name { get; set; }
    }
}