using GraphQLDemo.API.DataLoader;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Models.Common;
using GraphQLDemo.API.Models.Domain;
using HotChocolate;
using HotChocolate.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLDemo.API.DTOs
{
    public class CourseDTO
    {
        public Guid Id { get; set; }
        [IsProjected(false)] // avoid unwanted field projection to database.
        public string Name { get; set; }
        public Subject Subject { get; set; }
        
        [IsProjected(true)]
        public Guid InstructorId { get; set; }

        [GraphQLNonNullType]
        public async Task<InstructorDTO> Instructor([Service] InstructorDataLoader instructorDataLoader)
        {
            Instructor instructor = await instructorDataLoader.LoadAsync(InstructorId, CancellationToken.None); // here InstructorId is single key but you can pass list of keys here as well to dataloader.

            return new InstructorDTO()
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Salary = instructor.Salary
            };
        }

        public IEnumerable<StudentDTO> Students { get; set; }
    }
}
