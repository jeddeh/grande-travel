using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GrandeTravel.Data;
using GrandeTravel.Entity;
using GrandeTravel.Manager;
using GrandeTravel.Service;

using Moq;

using DeepEqual; // https://github.com/jamesfoster/DeepEqual
using DeepEqual.Syntax;

namespace GrandeTravel.Tests
{
    [TestClass]
    public class EntityTest
    {
        private ApplicationUser GetSampleUser(int id)
        {
            return new ApplicationUser
            {
                ApplicationUserId = id,
                Address = "5 Sample Street",
                City = "Sydney",
                Email = "email@sample.com",
                FirstName = "John",
                LastName = "Stanton",
                Phone = "0400606850",
                Postcode = "2010",
                State = Entity.Enums.AustralianStateEnum.NSW
            };
        }

        private Package GetSamplePackage(int id)
        {
            return new Package
            {
                PackageId = id,
                Accomodation = "Sample Accomodation",
                City = "Melbourne",
                ImageUrl = "~/Images/sample",
                Name = "Sample Package",
                Price = 900.0m,
                State = Entity.Enums.AustralianStateEnum.QLD,
                Status = Entity.Enums.PackageStatusEnum.Available
            };
        }

        public void Get_Manager_Test<T>(Func<int, T> getEntity, int id) where T : class
        {
            // Arrange:
            T sampleEntity = getEntity(id);

            Mock<IRepository<T>> mock = new Mock<IRepository<T>>();
            mock.Setup<T>(m => m.Get(id)).Returns(getEntity(id));

            IManager<T> manager = ManagerFactory.GetManager<T>(mock.Object);

            // Act:
            var actual = manager.GetById(id);
            // actual.ApplicationUserId = 2; // Expected to fail

            // Assert:
            sampleEntity.ShouldDeepEqual(actual);
        }

        [TestMethod]
        public void GetApplicationUser_Manager_Test()
        {
            Get_Manager_Test<ApplicationUser>(GetSampleUser, 1);
        }

        [TestMethod]
        public void GetPackage_Manager_Test()
        {
            Get_Manager_Test<Package>(GetSamplePackage, 5);
        }

        [TestMethod]
        public void GetApplicationUser_Service_Test()
        {
            // Arrange:
            int id = 1;

            ApplicationUser sampleUser = GetSampleUser(id);

            Mock<IManager<ApplicationUser>> mock = new Mock<IManager<ApplicationUser>>();
            mock.Setup<ApplicationUser>(m => m.GetById(id)).Returns(GetSampleUser(id));

            IApplicationUserService service = ServiceFactory.GetApplicationUserService(mock.Object);

            // Act:
            var actual = service.GetApplicationUserById(id);

            // Assert:
            Assert.AreEqual(ResultEnum.Success ,actual.Status);
            sampleUser.ShouldDeepEqual(actual.Data);
        }
    }
}

