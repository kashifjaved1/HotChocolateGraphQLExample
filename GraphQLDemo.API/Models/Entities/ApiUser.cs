using Microsoft.AspNetCore.Identity;

namespace GraphQLDemo.API.Models.Entities
{
    public class ApiUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
