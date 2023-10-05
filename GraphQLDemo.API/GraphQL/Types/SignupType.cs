using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraphQLDemo.API.GraphQL.Types
{
    public class SignupType
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public List<string> Permissions { get; set; }
    }
}
