using System.Threading.Tasks;


namespace Shop.Module.Core.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string body, bool isHtml = false);
    }
}
