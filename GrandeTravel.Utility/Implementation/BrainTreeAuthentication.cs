using Braintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Utility.Implementation
{
    public class BrainTreeAuthentication
    {
        public string MerchantId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }

        internal BraintreeGateway GetGateway()
        {
            return new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = this.MerchantId,
                PublicKey = this.PublicKey,
                PrivateKey = this.PrivateKey
            };
        }
    }
}
