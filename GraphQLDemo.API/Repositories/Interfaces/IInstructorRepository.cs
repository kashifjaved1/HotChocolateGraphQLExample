using GraphQLDemo.API.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace GraphQLDemo.API.Repositories.Interfaces
{
    public interface IInstructorRepository
    {
        public Task<Instructor> GetInstructorById(Guid instructorId);
        public Task<IEnumerable<Instructor>> GetManyById(IReadOnlyList<Guid> instructorIds);

    }
}
