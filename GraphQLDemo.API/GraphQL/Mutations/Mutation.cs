using AppAny.HotChocolate.FluentValidation;
using FluentValidation.Results;
using GraphQLDemo.API.GraphQL.Types;
using GraphQLDemo.API.Models.Entities;
using GraphQLDemo.API.Repositories;
using GraphQLDemo.API.Services.Interfaces;
using GraphQLDemo.API.Validators;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.GraphQL.Mutations
{
    public class Mutation
    {
        #region Account
        private readonly SignInManager<ApiUser> _signInManager;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IAuthManager _authManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly CourseRepository _courseRepository;
        //private readonly CourseTypeInputValidator _courseTypeInputValidator; // manually adding validator this is way is odd approach.

        public Mutation(UserManager<ApiUser> userManager, SignInManager<ApiUser> signInManager, IAuthManager authManager, RoleManager<IdentityRole> roleManager, CourseRepository courseRepository/*, CourseTypeInputValidator courseTypeInputValidator*/) // using validator manually)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authManager = authManager;
            _roleManager = roleManager;

            _courseRepository = courseRepository;
            //_courseTypeInputValidator = courseTypeInputValidator; // using validator manually
        }

        public async Task<bool> SignUp(SignupType signUp)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var isRoleExist = roles.FirstOrDefault(x => x.Name == signUp.Role) != null ? true : false;
            if (isRoleExist)
            {
                if (await _userManager.FindByEmailAsync(signUp.Email) == null)
                {
                    var user = new ApiUser
                    {
                        FullName = signUp.FullName,
                        UserName = signUp.Email,
                        Email = signUp.Email
                    };

                    var result = await _userManager.CreateAsync(user, signUp.Password);
                    user = await _userManager.FindByEmailAsync(signUp.Email);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, signUp.Role);
                    }

                    return true;
                }

                return false;
            }

            return false;
        }
        #endregion

        [Authorize]
        public async Task<CourseResult> CreateCourse([UseFluentValidation, UseValidator(typeof(CourseTypeInputValidator))] CourseTypeInput courseInput/*, [Service] ITopicEventSender topicEventSender*/) // [UseFluentValidation, UseValidator] attribute (method DI injection) from appany pkg will apply the validator automatically.
        {
            //Validate(courseInput); // using validator manually

            Course course = new Course()
            {
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

        [Authorize]
        public async Task<CourseResult> UpdateCourse(Guid id, [UseFluentValidation, UseValidator(typeof(CourseTypeInputValidator))] CourseTypeInput courseInput/*, [Service] ITopicEventSender topicEventSender*/)
        {
            //Validate(courseInput); // using validator manually

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

        [Authorize]
        public async Task<bool> DeleteCourse(Guid id)
        {
            return await _courseRepository.DeleteCourse(id);
        }

        //private void Validate(CourseTypeInput input)
        //{
        //    ValidationResult validationResult = _courseTypeInputValidator.Validate(input);
        //    if (!validationResult.IsValid) throw new GraphQLException("Invalid Input");
        //}
    }
}
