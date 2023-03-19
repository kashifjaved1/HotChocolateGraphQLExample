using GraphQLDemo.API.Models;
using GraphQLDemo.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Repositories
{
    public class InstructorRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> _dbContextFactory;

        public InstructorRepository(IDbContextFactory<SchoolDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Instructor> GetInstructorById(Guid id)
        {
            using (SchoolDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Instructors.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<IEnumerable<Instructor>> GetManyById(IReadOnlyList<Guid> instructorIds)
        {
            using (SchoolDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Instructors.Where(i => instructorIds.Contains(i.Id)).ToListAsync();
            }
        }
    }
}
