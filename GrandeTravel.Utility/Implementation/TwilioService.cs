using System;
using System.Threading;
using System.Threading.Tasks;
using Twilio;

namespace GrandeTravel.Utility.Implementation
{
    internal class TwilioService : IPhoneService
    {
        // Fields
        private readonly TwilioAuthentication authentication;

        // Constructor
        internal TwilioService(TwilioAuthentication authentication)
        {
            this.authentication = authentication;
        }

        // Methods
        public Task SendSMS(string phoneNumber, string message)
        {
            return Task.Run(() =>
                {
                    try
                    {
                        TwilioRestClient twilio = 
                            new TwilioRestClient(authentication.AccountSid, authentication.AccountSid);

                        twilio.SendSmsMessage(authentication.TwilioPhoneNumber, phoneNumber, message);
                    }
                    catch 
                    {
                    }
                }
            );
        }
    }
}