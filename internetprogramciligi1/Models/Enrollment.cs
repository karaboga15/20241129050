using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace internetprogramciligi1.Models
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } // Hangi Üye?

        public int CourseId { get; set; } // Hangi Kurs?

        public DateTime EnrollmentDate { get; set; } = DateTime.Now; // Ne zaman kaydoldu?

        // İlişkiler
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        [ForeignKey("UserId")]
        public virtual IdentityUser User { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}