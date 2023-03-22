using GraphQLDemo.API.DataLoader;
using GraphQLDemo.API.Models.Common;
using GraphQLDemo.API.Models.Entities;
using HotChocolate;
using HotChocolate.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLDemo.API.GraphQL.Types
{
    public class CourseType
    {
        public Guid Id { get; set; }
        [IsProjected(false)] // avoid unwanted field projection to database.
        public string Name { get; set; }
        public Subject Subject { get; set; }

        [IsProjected(true)]
        public Guid InstructorId { get; set; }

        [GraphQLNonNullType]
        public async Task<InstructorType> Instructor([Service] InstructorDataLoader instructorDataLoader)
        {
            Instructor instructor = await instructorDataLoader.LoadAsync(InstructorId, CancellationToken.None); // here InstructorId is single key but you can pass list of keys here as well to dataloader.

            return new InstructorType()
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Salary = instructor.Salary
            };
        }

        public IEnumerable<StudentType> Students { get; set; }
    }
}
