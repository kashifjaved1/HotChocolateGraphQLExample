using GraphQLDemo.API.Models.Entities;
using GraphQLDemo.API.Repositories;
using System;
using System.Threading.Tasks;

namespace GraphQLDemo.API.GraphQL.Mutations
{
    public class Mutation
    {
        private readonly CourseRepository _courseRepository;

        public Mutation(CourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<CourseResult> CreateCourse(CourseTypeInput courseInput/*, [Service] ITopicEventSender topicEventSender*/)
        {
            Course course = new Course()
            {
                //Id = Guid.NewGuid(),
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                InstructorId = courseInput.InstructorId

            };

            course = await _courseRepository.CreateCourseAsync(course);
            //await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);

            CourseResult result = new CourseResult
            {
                Id = course.Id,
                Name = course.Name,
                Subject = course.Subject,
                InstructorId = course.InstructorId
            };

            return result;
        }

        public async Task<CourseResult> UpdateCourse(Guid id, CourseTypeInput courseInput/*, [Service] ITopicEventSender topicEventSender*/)
        {
            var isCourseExist = _courseRepository.FindCourseById(id) != null;
            if (isCourseExist)
            {
                var course = new Course
                {
                    Id = id,
                    Name = courseInput.Name,
                    Subject = courseInput.Subject,
                    InstructorId = courseInput.InstructorId
                };

                var result = await _courseRepository.UpdateCourseAsync(course);
                return new CourseResult
                {
                    Id = result.Id,
                    Name = result.Name,
                    Subject = result.Subject,
                    InstructorId = result.InstructorId
                };
            }
            return new CourseResult();
        }

        public async Task<bool> DeleteCourse(Guid id)
        {
            return await _courseRepository.DeleteCourse(id);
        }
    }
}
