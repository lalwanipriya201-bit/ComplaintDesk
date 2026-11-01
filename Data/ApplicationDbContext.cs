using Microsoft.EntityFrameworkCore;
using ComplaintMgtSystem.Models;

namespace ComplaintMgtSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Complaint> Complaints { get; set; }

        public DbSet<ComplaintComment> ComplaintComments { get; set; }

        public DbSet<ComplaintStatusHistory> ComplaintStatusHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "IT",
                    Description = "Computer, software, internet and technical issues"
                },
                new Category
                {
                    Id = 2,
                    Name = "Infrastructure",
                    Description = "Classroom, building and infrastructure related issues"
                },
                new Category
                {
                    Id = 3,
                    Name = "Library",
                    Description = "Library and book related issues"
                },
                new Category
                {
                    Id = 4,
                    Name = "Hostel",
                    Description = "Hostel and accommodation related issues"
                },
                new Category
                {
                    Id = 5,
                    Name = "Academic",
                    Description = "Academic, subject and faculty related issues"
                },
                new Category
                {
                    Id = 6,
                    Name = "Transport",
                    Description = "Bus and transportation related issues"
                },
                new Category
                {
                    Id = 7,
                    Name = "Other",
                    Description = "Other complaints"
                }
            );

            // User → Complaints
            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.User)
                .WithMany(u => u.Complaints)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Category → Complaints
            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Complaints)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Complaint → Comments
            modelBuilder.Entity<ComplaintComment>()
                .HasOne(c => c.Complaint)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.ComplaintId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → Comments
            modelBuilder.Entity<ComplaintComment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Complaint → Status History
            modelBuilder.Entity<ComplaintStatusHistory>()
                .HasOne(s => s.Complaint)
                .WithMany(c => c.StatusHistory)
                .HasForeignKey(s => s.ComplaintId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → Status History
            modelBuilder.Entity<ComplaintStatusHistory>()
                .HasOne(s => s.ChangedByUser)
                .WithMany()
                .HasForeignKey(s => s.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}