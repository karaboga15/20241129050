using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace internetprogramciligi1.Models
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ders Başlığı")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Video Linki")]
        public string VideoUrl { get; set; } // Youtube Linki

        // Hangi Kursa Ait?
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
    }
}