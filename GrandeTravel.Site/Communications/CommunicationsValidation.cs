using System;
using System.Text.RegularExpressions;

namespace GrandeTravel.Site.Communications
{
    public static class CommunicationsValidation
    {
        private static readonly string AUSTRALIAN_PREFIX = "+61";

        public static string ValidateMobileNumber(string phoneNumber)
        {
            try
            {
                if (phoneNumber == null || phoneNumber.Length < 10)
                {
                    return null;
                }

                // Strip prefix and validate
                if (phoneNumber.StartsWith("0"))
                {
                    phoneNumber = phoneNumber.Substring(1);
                }
                else if (phoneNumber.StartsWith(AUSTRALIAN_PREFIX))
                {
                    phoneNumber = phoneNumber.Substring(3);
                }
                else
                {
                    return null;
                }

                // Get rid of non-numeric characters
                phoneNumber = Regex.Replace(phoneNumber, @"[^\d]", "");

                // Add prefix and validate length
                phoneNumber = AUSTRALIAN_PREFIX + phoneNumber;

                if (phoneNumber.Length == 12)
                {
                    return phoneNumber;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}