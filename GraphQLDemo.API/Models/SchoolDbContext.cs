using GraphQLDemo.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.Models
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasKey(x => x.Id);
            modelBuilder.Entity<Instructor>().HasKey(x => x.Id);
            modelBuilder.Entity<Student>().HasKey(x => x.Id);
        }
    }
}
