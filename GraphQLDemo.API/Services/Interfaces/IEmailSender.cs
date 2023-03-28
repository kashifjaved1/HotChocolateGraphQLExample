using GraphQLDemo.API.Services.Helpers;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Services.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
