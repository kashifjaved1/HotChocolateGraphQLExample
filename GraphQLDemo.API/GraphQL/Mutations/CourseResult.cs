using GraphQLDemo.API.GraphQL.Types;
using GraphQLDemo.API.Models.Common;
using System;

namespace GraphQLDemo.API.GraphQL.Mutations
{
    public class CourseResult : ISearchResultType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }
    }
}
