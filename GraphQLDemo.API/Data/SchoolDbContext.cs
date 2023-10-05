using GraphQLDemo.API.Data.Entities;
using GraphQLDemo.API.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace GraphQLDemo.API.Data
{
    public class SchoolDbContext : IdentityDbContext<ApiUser>
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Permission> Permissions { get; set; }

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

            builder.Entity<Permission>().HasData(
                new Permission { Id = Guid.NewGuid(), Name = "Permissions.Create" },
                new Permission { Id = Guid.NewGuid(), Name = "Permissions.View" },
                new Permission { Id = Guid.NewGuid(), Name = "Permissions.Edit" },
                new Permission { Id = Guid.NewGuid(), Name = "Permissions.Delete" }
            );
        }
    }
}
