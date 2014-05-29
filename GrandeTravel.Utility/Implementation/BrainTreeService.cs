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
                else if (result.Transaction != null)
                {
                    paymentResult.IsSuccess = false;
                    paymentResult.PaymentError = new PaymentError();

                    paymentResult.PaymentError.ErrorType = PaymentErrorTypeEnum.ProcessingError;
                    paymentResult.PaymentError.ErrorMessage = "Message: " + result.Message;
                    Transaction transaction = result.Transaction;
                    paymentResult.TransactionId = transaction.Id;

                    paymentResult.PaymentError.ErrorMessage += "\n\nError processing transaction:" +
                        "\n  Status: " + transaction.Status +
                        "\n  Code: " + transaction.ProcessorResponseCode +
                        "\n  Text: " + transaction.ProcessorResponseText;
                }
                else
                {
                    paymentResult.IsSuccess = false;
                    paymentResult.PaymentError = new PaymentError();

                    paymentResult.PaymentError.ErrorType = PaymentErrorTypeEnum.NoTransaction;
                    paymentResult.PaymentError.ErrorMessage = "Message: " + result.Message;

                    foreach (ValidationError error in result.Errors.DeepAll())
                    {
                        paymentResult.PaymentError.ErrorMessage += "\n\nAttribute: " + error.Attribute +
                        "\n  Code: " + error.Code +
                        "\n  Message: " + error.Message;
                    }
                }
            }
            catch (Exception e)
            {
                paymentResult.IsSuccess = false;
                paymentResult.PaymentError = new PaymentError();

                paymentResult.PaymentError.ErrorType = PaymentErrorTypeEnum.ApplicationError;
                paymentResult.PaymentError.ErrorMessage = e.Message;
            }

            return paymentResult;
        }
    }
}
