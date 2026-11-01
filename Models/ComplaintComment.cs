using System.ComponentModel.DataAnnotations;

namespace ComplaintMgtSystem.Models
{
    public class ComplaintComment
    {
        public int Id { get; set; }

        [Required]
        public int ComplaintId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public Complaint? Complaint { get; set; }

        public User? User { get; set; }
    }
}