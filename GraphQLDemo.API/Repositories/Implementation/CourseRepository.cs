using GraphQLDemo.API.Data;
using GraphQLDemo.API.Data.Entities;
using GraphQLDemo.API.Repositories.Interfaces;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Repositories.Implementation
{
    public class CourseRepository : ICourseRepository
    {
        //private readonly IDbContextFactory<SchoolDbContext> _dbContextFactory;
        private readonly SchoolDbContext _context;

        public CourseRepository(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<Course> CreateCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            try
            {
                _context.Courses.Update(course);
                await _context.SaveChangesAsync();
                return course;
            }
            catch (Exception ex)
            {
                throw new GraphQLException(ex.Message);
            }
        }

        public async Task<bool> DeleteCourse(Guid id)
        {
            Course course = new Course
            {
                Id = id
            };

            _context.Courses.Remove(course);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Course> FindCourseById(Guid id)
        {
            var course = await _context.Courses
                    // .Include(x => x.Instructor) // ignoring because dataloading will handle n+1 problem causing here.
                    .Include(x => x.Students)
                    .FirstOrDefaultAsync(x => x.Id == id);
            return course;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            try
            {
                var course = await _context.Courses
                    // .Include(x => x.Instructor) // ignoring because dataloading will handle n+1 problem causing here.
                    .Include(x => x.Students)
                    .ToListAsync();

                return course;
            }
            catch (Exception ex)
            {
                throw new GraphQLException(ex.Message);
            }
        }
    }
}
