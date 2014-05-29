using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Utility
{
    public class PaymentResult
    {
        public bool IsSuccess { get; set; }
        public string TransactionId { get; set; }
        public PaymentError PaymentError { get; set; }
    }

    public class PaymentError
    {
        public string ErrorMessage { get; set; }
        public PaymentErrorTypeEnum ErrorType { get; set; }
    }

    public enum PaymentErrorTypeEnum
    {
        ProcessingError,
        NoTransaction,
        ApplicationError
    }
}
