using System.ComponentModel.DataAnnotations;

namespace GraphQLDemo.API.GraphQL.Types
{
    public class SignupType
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
