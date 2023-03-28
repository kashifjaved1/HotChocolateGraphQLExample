using GraphQLDemo.API.Models.Entities;
using GraphQLDemo.API.Repositories.Implementation;
using GreenDonut;
using HotChocolate.DataLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLDemo.API.DataLoader
{
    public class InstructorDataLoader : BatchDataLoader<Guid, Instructor>
    {
        private readonly InstructorRepository _instructorRepository;

        public InstructorDataLoader(
            IBatchScheduler batchScheduler,
            InstructorRepository instructorRepository,
            DataLoaderOptions<Guid> options = null
        ) : base(batchScheduler, options)
        {
            _instructorRepository = instructorRepository;
        }

        // solving N+1 problem here. 
        protected override async Task<IReadOnlyDictionary<Guid, Instructor>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken) // collecting instructor requests at one place and will execute them as batch, once only.
        {
            var instructors = await _instructorRepository.GetManyById(keys);
            return instructors.ToDictionary(i => i.Id); // mapping instructorId -> guid to Instructor obj.
        }
    }
}
