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
        public Task SendSMS(string phoneNumber, string message)
        {
            return Task.Run(() =>
                {
                    try
                    {
                        // Assert.AreEqual("AC626c6b14ab61e8828467eb34d10b39c9", authentication.AccountSid);
                        // Assert.AreEqual("69fe587a51c8f48e92a68fcbd9b48477", authentication.AuthToken);
                        // Assert.AreEqual("(678) 394-0305", authentication.TwilioPhoneNumber);

                        TwilioRestClient twilio = 
                            new TwilioRestClient(authentication.AccountSid, authentication.AccountSid);

                        SMSMessage result = twilio.SendSmsMessage(authentication.TwilioPhoneNumber, phoneNumber, message);
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