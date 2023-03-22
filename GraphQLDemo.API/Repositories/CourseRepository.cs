using GraphQLDemo.API.Models;
using GraphQLDemo.API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Repositories
{
    public class CourseRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> _dbContextFactory;

        public CourseRepository(IDbContextFactory<SchoolDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Course> CreateCourseAsync(Course course)
        {
            using (SchoolDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Courses.Add(course);
                await context.SaveChangesAsync();
                return course;
            }
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            using (SchoolDbContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    context.Courses.Update(course);
                    await context.SaveChangesAsync();
                    return course;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteCourse(Guid id)
        {
            using (SchoolDbContext context = _dbContextFactory.CreateDbContext())
            {
                Course course = new Course
                {
                    Id = id
                };

                context.Courses.Remove(course);
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<Course> FindCourseById(Guid id)
        {
            using (SchoolDbContext context = _dbContextFactory.CreateDbContext())
            {
                var course = await context.Courses
                    // .Include(x => x.Instructor) // ignoring because dataloading will handle n+1 problem causing here.
                    .Include(x => x.Students)
                    .FirstOrDefaultAsync(x => x.Id == id);
                return course;
            }
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            using (SchoolDbContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    var course = await context.Courses
                        // .Include(x => x.Instructor) // ignoring because dataloading will handle n+1 problem causing here.
                        .Include(x => x.Students)
                        .ToListAsync();

                    return course;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
