using GraphQLDemo.API.Data.Common;
using System;
using System.Collections.Generic;

namespace GraphQLDemo.API.Data.Entities
{
    public class Course
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public Subject Subject { get; set; }

        public Guid InstructorId { get; set; } // Instructor PK to make sure there must be valid instructor for the course for mutation operations.
        public Instructor Instructor { get; set; }

        public IEnumerable<Student> Students { get; set; }
    }
}
