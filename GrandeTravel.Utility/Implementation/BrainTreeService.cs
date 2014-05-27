using Braintree;
using Braintree.Exceptions;
using GrandeTravel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Utility.Implementation
{
    internal class BrainTreeService : IPaymentService
    {
        // Fields
        private readonly BrainTreeAuthentication authentication;

        // Constructor
        internal BrainTreeService(BrainTreeAuthentication authentication)
        {
            this.authentication = authentication;
        }

        // Methods
        public PaymentResult SubmitPayment(Payment payment)
        {
            PaymentResult paymentResult = new PaymentResult();

            try
            {
                TransactionRequest request = new TransactionRequest
                {
                    Amount = payment.Amount,
                    CreditCard = new TransactionCreditCardRequest
                    {
                        Number = payment.CCNumber,
                        CVV = payment.CVV,
                        ExpirationMonth = payment.ExpirationMonth,
                        ExpirationYear = payment.ExpirationYear
                    },
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    }
                };

                Result<Transaction> result = authentication.GetGateway().Transaction.Sale(request);

                if (result.IsSuccess())
                {
                    Transaction transaction = result.Target;
                    paymentResult.IsSuccess = true;
                    paymentResult.TransactionId = transaction.Id;
                }
                else
                {
                    paymentResult.IsSuccess = false;
                    paymentResult.TransactionId = null;
                }
            }
            catch
            {
                paymentResult.IsSuccess = false;
                paymentResult.TransactionId = null;
            }

            return paymentResult;
        }
    }
}
