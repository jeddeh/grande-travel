using GrandeTravel.Utility.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace GrandeTravel.Site.Helpers
{
    public static class AuthenticationFactory
    {
        public static BrainTreeAuthentication GetBrainTreeAuthentication()
        {
            return new BrainTreeAuthentication
                {
                    MerchantId = WebConfigurationManager.AppSettings["brainTreeMerchantId"],
                    PrivateKey = WebConfigurationManager.AppSettings["brainTreePrivateKey"],
                    PublicKey = WebConfigurationManager.AppSettings["brainTreePublicKey"]
                };
        }

        public static TwilioAuthentication GetTwilioAuthentication()
        {
            return new TwilioAuthentication
                {
                    AccountSid = WebConfigurationManager.AppSettings["twilioAccountSid"],
                    AuthToken = WebConfigurationManager.AppSettings["twilioAuthToken"],
                    TwilioPhoneNumber = WebConfigurationManager.AppSettings["twilioPhoneNumber"]
                };
        }

        public static DefaultEmailAuthentication GetDefaultEmailAuthentication()
        {
            return new DefaultEmailAuthentication
            {
                Host = WebConfigurationManager.AppSettings["defaultEmailHost"],
                Port = Int32.Parse(WebConfigurationManager.AppSettings["defaultEmailPort"]),
                UserName = WebConfigurationManager.AppSettings["defaultEmailUserName"],
                Password = WebConfigurationManager.AppSettings["defaultEmailPassword"]
            };
        }
    }
}