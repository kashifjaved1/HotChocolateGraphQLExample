using System.ComponentModel.DataAnnotations;

namespace GraphQLDemo.API.GraphQL.Types
{
    public class LoginType
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
