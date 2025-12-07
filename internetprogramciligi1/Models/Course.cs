using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace internetprogramciligi1.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kurs adı boş geçilemez.")]
        [Display(Name = "Kurs Başlığı")]
        public string Title { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Display(Name = "Resim (URL)")]
        public string? ImageUrl { get; set; }

        // --- Kategori İlişkisi ---
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        // --- Eğitmen İlişkisi ---
        [Display(Name = "Eğitmen")]
        public int? InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public virtual Instructor? Instructor { get; set; }
    }
}