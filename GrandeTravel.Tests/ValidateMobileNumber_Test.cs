using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GrandeTravel.Site.Communications;

namespace GrandeTravel.Tests
{
    /// <summary>
    /// Unit tests for validating an Australian mobile number and adding an international prefix.
    /// </summary>
    [TestClass]
    public class ValidateMobileNumber_Test
    {
        [TestMethod]
        public void ValidateMobileNumber_Test1()
        {
            string phoneNumber = "0401606888";
            string actual = CommunicationsValidation.ValidateMobileNumber(phoneNumber);
            Assert.AreEqual("+61401606888", actual);
        }

        [TestMethod]
        public void ValidateMobileNumber_Test2()
        {
            string phoneNumber = "0401 606888";
            string actual = CommunicationsValidation.ValidateMobileNumber(phoneNumber);
            Assert.AreEqual("+61401606888", actual);
        }

        public void ValidateMobileNumber_Test3()
        {
            string phoneNumber = "(0401) 606888";
            string actual = CommunicationsValidation.ValidateMobileNumber(phoneNumber);
            Assert.AreEqual("+61401606888", actual);
        }

        [TestMethod]
        public void ValidateMobileNumber_Test4()
        {
            string phoneNumber = "+61 401 606 888 ";
            string actual = CommunicationsValidation.ValidateMobileNumber(phoneNumber);
            Assert.AreEqual("+61401606888", actual);
        }

        [TestMethod]
        public void ValidateMobileNumber_Test5()
        {
            string phoneNumber = "+62654654654";
            string actual = CommunicationsValidation.ValidateMobileNumber(phoneNumber);
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void ValidateMobileNumber_Test6()
        {
            string phoneNumber = "034016 06888";
            string actual = CommunicationsValidation.ValidateMobileNumber(phoneNumber);
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void ValidateMobileNumber_Test7()
        {
            string phoneNumber = "+61";
            string actual = CommunicationsValidation.ValidateMobileNumber(phoneNumber);
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void ValidateMobileNumber_Test8()
        {
            string phoneNumber = "";
            string actual = CommunicationsValidation.ValidateMobileNumber(phoneNumber);
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void ValidateMobileNumber_Test9()
        {
            string phoneNumber = null;
            string actual = CommunicationsValidation.ValidateMobileNumber(phoneNumber);
            Assert.AreEqual(null, actual);
        }
    }
}

