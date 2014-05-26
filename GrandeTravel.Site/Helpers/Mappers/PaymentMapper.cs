using GrandeTravel.Entity;
using GrandeTravel.Site.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrandeTravel.Site.Helpers.Mappers
{
    public static class PaymentMapper
    {
        public static Payment ToPayment(this PaymentViewModel model)
        {
            return new Payment
            {
                CCNumber = model.CCNumber,
                CVV = model.CVV,
                ExpirationMonth = model.ExpirationMonth,
                ExpirationYear = model.ExpirationYear,
                Amount = model.Amount,
                PackageId = model.PackageId,
                PackageName = model.PackageName
            };
        }
    }
}