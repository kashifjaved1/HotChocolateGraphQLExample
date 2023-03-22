using GraphQLDemo.API.Models.Entities;
using HotChocolate.Data.Filters;

namespace GraphQLDemo.API.GraphQL.Filters
{
    public class CourseFilterType : FilterInputType<Course>
    {
        protected override void Configure(IFilterInputTypeDescriptor<Course> descriptor)
        {
            descriptor.Ignore(c => c.Students);
            base.Configure(descriptor);
        }
    }
}
