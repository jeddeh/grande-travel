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
                City = "Sydney",
                State = AustralianStateEnum.NSW,
                Accomodation = "5 nights at Petersham TAFE, Sydney"
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
                ConfirmPassword = "Aa111111",
                Email = "andrewjones" + randomNumber + "@aj.com.au",
                FirstName = "Andrew",
                LastName = "Jones",
                Password = "Aa111111",
                Phone = "0434 412 749",
                Postcode = "2016",
                State = AustralianStateEnum.WA
            };
        }
    }
}