using GraphQLDemo.API.Data;
using GraphQLDemo.API.Data.Entities;
using GraphQLDemo.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Repositories.Implementation
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly SchoolDbContext _context;

        public InstructorRepository(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<Instructor> GetInstructorById(Guid instructorId)
        {
            return await _context.Instructors.FirstOrDefaultAsync(x => x.Id == instructorId);
        }

        public async Task<IEnumerable<Instructor>> GetManyById(IReadOnlyList<Guid> instructorIds)
        {
            return await _context.Instructors.Where(i => instructorIds.Contains(i.Id)).ToListAsync();
        }
    }
}
