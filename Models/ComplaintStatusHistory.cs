using System.ComponentModel.DataAnnotations;

namespace ComplaintMgtSystem.Models
{
    public class ComplaintStatusHistory
    {
        public int Id { get; set; }

        [Required]
        public int ComplaintId { get; set; }

        [StringLength(50)]
        public string? OldStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string NewStatus { get; set; } = string.Empty;

        [Required]
        public int ChangedBy { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.Now;

        [StringLength(1000)]
        public string? Remark { get; set; }

        // Navigation property
        public Complaint? Complaint { get; set; }

        public User? ChangedByUser { get; set; }
    }
}