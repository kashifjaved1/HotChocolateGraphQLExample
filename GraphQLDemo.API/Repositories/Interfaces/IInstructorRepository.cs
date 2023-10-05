using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using GraphQLDemo.API.Data.Entities;

namespace GraphQLDemo.API.Repositories.Interfaces
{
    public interface IInstructorRepository
    {
        public Task<Instructor> GetInstructorById(Guid instructorId);
        public Task<IEnumerable<Instructor>> GetManyById(IReadOnlyList<Guid> instructorIds);

    }
}
