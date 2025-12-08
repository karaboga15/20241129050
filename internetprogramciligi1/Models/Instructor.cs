using System.ComponentModel.DataAnnotations;

namespace internetprogramciligi1.Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [Display(Name = "Adı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [Display(Name = "Soyadı")]
        public string Surname { get; set; }

        [Display(Name = "Ünvan / Uzmanlık")]
        public string? Title { get; set; } // Örn: Prof. Dr. veya Yazılım Uzmanı

        [Display(Name = "Profil Resmi")]
        public string? ImageUrl { get; set; }

        
        public string FullName => $"{Name} {Surname}";
    }
}