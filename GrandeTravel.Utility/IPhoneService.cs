using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Utility
{
    public interface IPhoneService
    {
        Task SendSMS(string message, string phoneNumber);
    }
}
