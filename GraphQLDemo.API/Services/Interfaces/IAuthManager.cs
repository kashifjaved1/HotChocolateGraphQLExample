using GraphQLDemo.API.GraphQL.Types;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Services.Interfaces
{
    public interface IAuthManager
    {
        public Task<string> CreateToken();
        Task<bool> ValidateUser(LoginType login);
    }
}
