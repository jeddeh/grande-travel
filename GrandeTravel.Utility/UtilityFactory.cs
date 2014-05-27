using GrandeTravel.Utility.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Utility
{
    public static class UtilityFactory
    {
        public static IPhoneService GetPhoneService(TwilioAuthentication authentication)
        {
            return new TwilioService(authentication);
        }

        public static IPaymentService GetBrainTreeService(BrainTreeAuthentication authentication)
        {
            return new BrainTreeService(authentication);
        }

        public static IEmailService GetEmailService(DefaultEmailAuthentication authentication)
        {
            return new DefaultEmailService(authentication);
        }
    }
}
