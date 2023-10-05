using GraphQLDemo.API.GraphQL.Types;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Models.Entities;
using GraphQLDemo.API.Services.Helpers;
using GraphQLDemo.API.Services.Interfaces;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.GraphQL.Queries
{
    public class Query
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IAuthManager _authManager;
        private readonly SignInManager<ApiUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Query(IAuthManager authManager, SignInManager<ApiUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, UserManager<ApiUser> userManager)
        {
            _authManager = authManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public async Task<string> Login(LoginType login)
        {
            if (!await _authManager.ValidateUser(login)) return null;
            return await _authManager.CreateToken();
        }

        [Authorize]
        public async Task<bool> Logout()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<bool> ForgotPassword(ResetPasswordType resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                //return false;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = "http://mydomain.com/resetPasswordPage?token=" + token + "&email=" + user.Email; // can be localhost for testing purposes on local machine.

            var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
            await _emailSender.SendEmailAsync(message);

            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordType resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                return false;
            }

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (!resetPassResult.Succeeded)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteAccount(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return true;
            }
            return false;
        }

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