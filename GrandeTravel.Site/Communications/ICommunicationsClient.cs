using System.Threading.Tasks;

namespace GrandeTravel.Site.Communications
{
    public interface ICommunicationsClient
    {
        Task SendSMS(string message, string phoneNumber);
    }
}
