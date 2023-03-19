using GraphQLDemo.API.Models;
using GraphQLDemo.API.Models.Common;
using GraphQLDemo.API.GraphQL.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.GraphQL.Mutations
{
    public class CourseTypeInput
    {
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }
    }
}
