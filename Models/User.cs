using System.ComponentModel.DataAnnotations;

namespace ComplaintMgtSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Complaint> Complaints { get; set; }
            = new List<Complaint>();

        public ICollection<ComplaintComment> Comments { get; set; }
            = new List<ComplaintComment>();
    }
}