using System;
using GraphQLDemo.API.Data.Common;

namespace GraphQLDemo.API.GraphQL.Mutations
{
    public class CourseTypeInput
    {
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }
    }
}
