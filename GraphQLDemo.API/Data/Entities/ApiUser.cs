using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace GraphQLDemo.API.Data.Entities
{
    public class ApiUser : IdentityUser
    {
        public string FullName { get; set; }

        public string Permissions { get; set; }
    }
}
