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

        public static BrainTreeService GetBrainTreeService(BrainTreeAuthentication authentication)
        {
            return new BrainTreeService(authentication);
        }
    }
}
