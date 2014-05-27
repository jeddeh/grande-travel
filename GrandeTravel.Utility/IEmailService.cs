using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Utility
{
    public interface IEmailService
    {
        Task SendEmail(Email email);
    }
}
