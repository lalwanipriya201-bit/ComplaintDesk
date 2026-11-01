using System.ComponentModel.DataAnnotations;

namespace ComplaintMgtSystem.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Description { get; set; }

        // Navigation property
        public ICollection<Complaint> Complaints { get; set; }
            = new List<Complaint>();
    }
}