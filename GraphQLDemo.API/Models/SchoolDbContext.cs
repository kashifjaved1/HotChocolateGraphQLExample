using GraphQLDemo.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Models
{
    public class SchoolDbContext : IdentityDbContext<ApiUser>
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>().HasKey(x => x.Id);
            modelBuilder.Entity<Instructor>().HasKey(x => x.Id);
            modelBuilder.Entity<Student>().HasKey(x => x.Id);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder builder)
        {
            // Seeding Roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                }
            );
        }
    }
}
