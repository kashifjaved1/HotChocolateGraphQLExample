using GraphQLDemo.API.Models;
using HotChocolate;
using HotChocolate.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDemo.API.GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.API.GraphQL.Queries
{
    public class Query
    {
        // All Course relevant queries are segregated and organized to their own courseQuery class.
        
        [UseDbContext(typeof(SchoolDbContext))]
        public async Task<IEnumerable<ISearchResultType>> Search(string searchTerm, [ScopedService] SchoolDbContext context) // use of interfaceType for shared property data.
        {
            IEnumerable<CourseType> courses = await context.Courses
                .Where(c => c.Name.Contains(searchTerm))
                .Select(c => new CourseType
                {
                    Id = c.Id,
                    Name = c.Name,
                    Subject = c.Subject,
                    InstructorId = c.InstructorId
                })
                .ToListAsync();

            IEnumerable<InstructorType> instructors = await context.Instructors
                .Where(i => i.FirstName.Contains(searchTerm) || i.LastName.Contains(searchTerm))
                .Select(i => new InstructorType
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    Salary = i.Salary
                }).ToListAsync();

            return new List<ISearchResultType>().Concat(courses).Concat(instructors);
        }

        [GraphQLDeprecated("This query is deprecated.")] // this attribute use for depricating.
        public string Instructions => "Smash that Star button and Fork my project.";
    }
}
