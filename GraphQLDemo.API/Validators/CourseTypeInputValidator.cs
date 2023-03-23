using FluentValidation;
using GraphQLDemo.API.GraphQL.Mutations;

namespace GraphQLDemo.API.Validators
{
    public class CourseTypeInputValidator : AbstractValidator<CourseTypeInput> // custom validator from fluentValidation for validation.
    {
        public CourseTypeInputValidator()
        {
            RuleFor(c => c.Name)
                .MinimumLength(3)
                .MaximumLength(50)
                .WithMessage("Course name must be within 3 - 50 characters.")
                .WithErrorCode("COURSE_NAME_LENGTH");
        }
    }
}
