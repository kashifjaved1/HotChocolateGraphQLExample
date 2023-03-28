using System.ComponentModel.DataAnnotations;

namespace GraphQLDemo.API.GraphQL.Types
{
    public class ForgotPasswordType
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class LoginType : ForgotPasswordType
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class ChangePasswordType : LoginType { }

    public class ResetPasswordType : LoginType
    {
        public string Token { get; set; }
    }
}
