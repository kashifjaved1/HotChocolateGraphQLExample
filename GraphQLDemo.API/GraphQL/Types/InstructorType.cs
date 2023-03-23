using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.GraphQL.Types
{
    public class InstructorType : ISearchResultType
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Salary { get; set; }

        public IEnumerable<CourseType> Courses { get; set; }
    }
}
