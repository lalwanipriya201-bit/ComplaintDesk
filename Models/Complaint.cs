using System.ComponentModel.DataAnnotations;

namespace ComplaintMgtSystem.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string ComplaintNumber { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(150)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Priority { get; set; } = "Medium";

        [Required]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public DateTime? ResolvedAt { get; set; }

        [StringLength(2000)]
        public string? AdminRemark { get; set; }

        // Navigation properties
        public User? User { get; set; }

        public Category? Category { get; set; }

        public ICollection<ComplaintComment> Comments { get; set; }
            = new List<ComplaintComment>();

        public ICollection<ComplaintStatusHistory> StatusHistory { get; set; }
            = new List<ComplaintStatusHistory>();
    }
}