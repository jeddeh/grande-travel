using System;
using System.Threading;
using System.Threading.Tasks;
using Twilio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GrandeTravel.Utility.Implementation
{
    internal class TwilioService : IPhoneService
    {
        // Fields
        private readonly TwilioAuthentication authentication;
        private SMSMessage result;

        // Constructor
        internal TwilioService(TwilioAuthentication authentication)
        {
            this.authentication = authentication;
        }

        // Methods
        public Task SendSMSAsync(string phoneNumber, string message)
        {
            return Task.Run(() =>
                {
                    try
                    {
                        TwilioRestClient twilio =
                            new TwilioRestClient(authentication.AccountSid, authentication.AccountSid);
                        result = twilio.SendSmsMessage(authentication.TwilioPhoneNumber, phoneNumber, message);
                    }
                    catch
                    {
                        string errorMessage = result.RestException.Message;
                    }
                }
            );
        }
    }
}