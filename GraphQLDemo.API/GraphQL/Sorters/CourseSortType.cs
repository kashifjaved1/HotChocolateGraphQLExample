using GraphQLDemo.API.Models.Entities;
using HotChocolate.Data.Sorting;

namespace GraphQLDemo.API.GraphQL.Sorters
{
    public class CourseSortType : SortInputType<Course>
    {
        protected override void Configure(ISortInputTypeDescriptor<Course> descriptor)
        {
            descriptor.Ignore(x => x.Id);  
            descriptor.Ignore(x => x.InstructorId);
            base.Configure(descriptor);
        }
    }
}
