using GraphQLDemo.API.Models;
using GraphQLDemo.API.Models.Common;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Models.Entities
{
    public class CourseType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }

        [GraphQLNonNullType]
        public InstructorType Instructor { get; set; }
        public IEnumerable<StudentType> Students { get; set; }
    }
}
