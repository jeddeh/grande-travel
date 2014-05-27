using GrandeTravel.Entity.Enums;
using GrandeTravel.Site.Models.Membership;
using GrandeTravel.Site.Models.Packages;
using GrandeTravel.Site.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrandeTravel.Site.Helpers
{
    public static class SampleModelData
    {
        public static PackagesViewModel GetSamplePackagesViewModel()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 10000);

            return new PackagesViewModel
            {
                PackageName = "Package " + randomNumber,
                Price = 900.00m,
                City = "Melbourne",
                State = AustralianStateEnum.VIC,
                Accomodation = "5 nights at the Grand Hotel, Melbourne"
            };
        }

        public static RegisterUserViewModel GetSampleRegisterViewModel()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 10000);

            return new RegisterUserViewModel
            {
                Address = "5 Short Street",
                City = "Sydney",
                ConfirmPassword = "111111",
                Email = "andrewjones" + randomNumber + "@aj.com.au",
                FirstName = "Andrew",
                LastName = "Jones",
                Password = "111111",
                Phone = "0400 000 000",
                Postcode = "2016",
                State = AustralianStateEnum.WA
            };
        }

        public static PaymentViewModel GetSamplePaymentViewModel()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 10000);

            return new PaymentViewModel
            {
                PackageId = 1,
                PackageName = "Sample Package",
                Amount = 1000.00m,
                CCNumber = "4111111111111111",
                CVV = "555",
                ExpirationMonth = "05",
                ExpirationYear = "2015"
            };
        }

    }
}