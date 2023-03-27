using GraphQLDemo.API.Models;
using GraphQLDemo.API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Repositories
{
    public class InstructorRepository
    {
        //private readonly IDbContextFactory<SchoolDbContext> _dbContextFactory;

        //public InstructorRepository(IDbContextFactory<SchoolDbContext> dbContextFactory)
        //{
        //    _dbContextFactory = dbContextFactory;
        //}

        private readonly SchoolDbContext _context;

        public InstructorRepository(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<Instructor> GetInstructorById(Guid id)
        {
            return await _context.Instructors.FirstOrDefaultAsync(x => x.Id == id);
            //using (SchoolDbContext context = _dbContextFactory.CreateDbContext())
            //{
            //    return await context.Instructors.FirstOrDefaultAsync(x => x.Id == id);
            //}
        }

        public async Task<IEnumerable<Instructor>> GetManyById(IReadOnlyList<Guid> instructorIds)
        {
            return await _context.Instructors.Where(i => instructorIds.Contains(i.Id)).ToListAsync();
            //using (SchoolDbContext context = _dbContextFactory.CreateDbContext())
            //{
            //    return await context.Instructors.Where(i => instructorIds.Contains(i.Id)).ToListAsync();
            //}
        }
    }
}
