namespace GrandeTravel.Data.Migrations
{
    using GrandeTravel.Entity;
    using GrandeTravel.Entity.Enums;

    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<GrandeTravel.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GrandeTravel.Data.ApplicationDbContext context)
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "Email", true);
            var roles = (SimpleRoleProvider)Roles.Provider;

            #region roles

            if (!roles.RoleExists("ActiveUser"))
            {
                roles.CreateRole("ActiveUser");
            }

            if (!roles.RoleExists("Admin"))
            {
                roles.CreateRole("Admin");
            }

            if (!roles.RoleExists("Customer"))
            {
                Roles.CreateRole("Customer");
            }

            if (!roles.RoleExists("Provider"))
            {
                Roles.CreateRole("Provider");
            }

            #endregion

            #region admin

            string adminEmail = "admin@sample.com";
            WebSecurity.CreateUserAndAccount(adminEmail, "111111");
            int adminId = WebSecurity.GetUserId(adminEmail);

            ApplicationUser[] admin = new ApplicationUser[]
            {
                new ApplicationUser 
                {
                    ApplicationUserId = adminId,
                    FirstName = "Claire",
                    LastName = "Rivera",
                    Address = "201 Walker Street",
                    City = "Townsville",
                    Postcode = "4810",
                    State = AustralianStateEnum.QLD,
                    Phone = "0401 605 651",
                    Email = adminEmail,
                }
            };

            context.ApplicationUsers.AddOrUpdate(admin);
            context.SaveChanges();

            Roles.AddUserToRole(adminEmail, "Admin");
            Roles.AddUserToRole(adminEmail, "ActiveUser");

            #endregion

            #region ApplicationUsers

            string email1 = "customer1@sample.com";
            WebSecurity.CreateUserAndAccount(email1, "111111");
            int ApplicationUserId1 = WebSecurity.GetUserId(email1);

            string email2 = "customer2@sample.com";
            WebSecurity.CreateUserAndAccount(email2, "111111");
            int ApplicationUserId2 = WebSecurity.GetUserId(email2);

            string email3 = "customer3@sample.com";
            WebSecurity.CreateUserAndAccount(email3, "111111");
            int ApplicationUserId3 = WebSecurity.GetUserId(email3);

            string email4 = "provider1@sample.com";
            WebSecurity.CreateUserAndAccount(email4, "111111");
            int ApplicationUserId4 = WebSecurity.GetUserId(email4);

            string email5 = "provider2@sample.com";
            WebSecurity.CreateUserAndAccount(email5, "111111");
            int ApplicationUserId5 = WebSecurity.GetUserId(email5);

            string email6 = "provider3@sample.com";
            WebSecurity.CreateUserAndAccount(email6, "111111");
            int ApplicationUserId6 = WebSecurity.GetUserId(email6);

            ApplicationUser[] ApplicationUsers = new ApplicationUser[]
            {
                new ApplicationUser 
                {
                    ApplicationUserId = ApplicationUserId1,
                    FirstName = "John",
                    LastName = "Stanton",
                    Address = "5 Short Street, Petersham",
                    City = "Sydney",
                    Postcode = "2049",
                    State = AustralianStateEnum.NSW,
                    Phone = "0401 605 650",
                    Email = email1,
                },

                new ApplicationUser 
                { 
                    ApplicationUserId = ApplicationUserId2,
                    FirstName = "Bridget",
                    LastName = "Jones",
                    Address = "6/10 Smith Street",
                    City = "Euroa",
                    Postcode = "3666",
                    State = AustralianStateEnum.VIC,
                    Phone = "0403 605 650",
                    Email = email2,
                },

                new ApplicationUser
                { 
                    ApplicationUserId = ApplicationUserId3,
                    FirstName = "Chris",
                    LastName = "Nguyen",
                    Address = "101 Harry's Place",
                    City = "Hobart",
                    Postcode = "7001",
                    State = AustralianStateEnum.TAS,
                    Phone = "0401 605 651",
                    Email = email3,
                },

               new ApplicationUser 
               {
                    ApplicationUserId = ApplicationUserId4,
                    FirstName = "Peter",
                    LastName = "Montgomery",
                    Address = "158 Walker Street",
                    City = "Townsville",
                    Postcode = "4810",
                    State = AustralianStateEnum.QLD,
                    Phone = "0403 605 650",
                    Email = email4,
               },

                new ApplicationUser
                { 
                    ApplicationUserId = ApplicationUserId5,
                    FirstName = "Julius",
                    LastName = "Dutton",
                    Address = "158 Georges Terrace",
                    City = "Oatlands Park",
                    Postcode = "5046",
                    State = AustralianStateEnum.SA,
                    Phone = "0403 605 650",
                    Email = email5,     
                },

                new ApplicationUser { 
                    ApplicationUserId = ApplicationUserId6,
                    FirstName = "Mary",
                    LastName = "Poulson",
                    Address = "101 Jareys Close",
                    City = "Hobart",
                    Postcode = "7001",
                    State = AustralianStateEnum.TAS,
                    Phone = "0401 605 651",
                    Email = email6,
                }
            };

            try
            {
                context.ApplicationUsers.AddOrUpdate(ApplicationUsers);
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var validationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            Roles.AddUsersToRoles(new string[] { email1, email2, email3 }, new string[] { "Customer", "ActiveUser" });
            Roles.AddUsersToRoles(new string[] { email4, email5, email6 }, new string[] { "Provider", "ActiveUser" });

            #endregion

            //#region packages

            //Activity[] activities = new Activity[]
            //{
            //    //new Activity {
            //    //    ActivityId = 1,
            //    //    Name = "Opera on Sydney Harbour",
            //    //    Description = "Indulge your love of opera performed on a shimmering stage on the waters of Sydney Harbour.",
            //    //    City = "Sydney",
            //    //    StartDate = new DateTime(2014,3,21),
            //    //    EndDate = new DateTime(2014,4,12),
            //    //    State = AustralianStateEnum.NSW,
            //    //    Accomodation = "2 nights at the Grace Hotel, Sydney",
            //    //    Price = 400.00m,
            //    //    ImageUrl = @"../../Images/Packages/OperaOnSydneyHarbour.jpg",
            //    //    ApplicationUserId = ApplicationUserId1,
            //    //    Status = PackageStatus.Available },

            //    new Activity {
            //        ActivityId = 2,
            //        Name = "Melbourne Food and Wine Festival",
            //        Description = "Indulge your taste buds with a world class program of over 300 culinary events filling Melbourne’s maze of hidden laneways, as well spectacular regional Victoria.",
            //        City = "Melbourne",
            //        StartDate = new DateTime(2014,2,28),
            //        EndDate = new DateTime(2014,3,16),
            //        State = AustralianStateEnum.VIC,
            //        Accomodation = "3 nights at the Hotel Windsor, Melbourne",
            //        Price = 300.00m,
            //        ImageUrl = @"../../Images/Packages/MelbourneFoodAndWine.jpg",
            //        ApplicationUserId = ApplicationUserId1,
            //        Status = PackageStatus.Available },

            //    new Activity {
            //        ActivityId = 3,
            //        Name = "Canberra Balloon Spectacular",
            //        Description = "See a stunning array of hot air balloons inflate at dawn and slowly drift over Canberra's iconic national attractions.",
            //        City = "Canberra",
            //        StartDate = new DateTime(2014,3,8),
            //        EndDate = new DateTime(2014,3,16),
            //        State = AustralianStateEnum.ACT,
            //        Accomodation = "2 nights at the Old Canberra Inn, Canberra",
            //        Price = 350.00m,
            //        ImageUrl = @"../../Images/Packages/Ballooning.jpg",
            //        ApplicationUserId = ApplicationUserId2,
            //        Status = PackageStatus.Available },

            //    //new Activity {
            //    //    ActivityId = 4,
            //    //    Name = "WOMADelaide",
            //    //    Description = "Enjoy the world’s best traditional and contemporary musicians, dancers and DJs in this outdoor festival held in Botanic Park in Adelaide.",
            //    //    City = "Adelaide",
            //    //    StartDate = new DateTime(2014,3,7),
            //    //    EndDate = new DateTime(2014,4,10),
            //    //    State = AustralianStateEnum.SA,
            //    //    Accomodation = "4 nights at the Majestic Roof Garden Hotel, Adelaide",
            //    //    Price = 500.00m,
            //    //    ImageUrl = @"../../Images/Packages/Womadelaide.jpg",
            //    //    ApplicationUserId = ApplicationUserId2,
            //    //    Status = PackageStatus.Available },

            //    new Activity {
            //        ActivityId = 5,
            //        Name = "Formula 1 Australian Grand Prix",
            //        Description = "Watch the world’s fastest cars compete in this thrilling Formula 1 race in Melbourne at the Albert Park circuit.",
            //        City = "Melbourne",
            //        State = AustralianStateEnum.VIC,
            //        Accomodation = "3 nights at the Swanston Hotel, Melbourne",
            //        Price = 400.00m,
            //        ImageUrl = @"../../Images/Packages/AustralianGrandPrix.jpg",
            //        ApplicationUserId = ApplicationUserId1,
            //        Status = PackageStatus.Discontinued },

            //    new Activity {
            //        ActivityId = 6,
            //        Name = "Sculptures by the Sea",
            //        Description = "The stunning Cottesloe Beach will be transformed into a spectacular sculpture park, featuring more than 70 artists from Australia and across the world.",
            //        City = "Cottesloe Beach",
            //        State = AustralianStateEnum.WA,
            //        Accomodation = "3 nights at the Mosman Park, Cottesloe",
            //        Price = 650.00m,
            //        ImageUrl = @"../../Images/Packages/SculpturesByTheSea.jpg",
            //        ApplicationUserId = ApplicationUserId1,
            //        Status = PackageStatus.Available },

            //    //new Activity {
            //    //    ActivityId = 7,
            //    //    Name = "Adelaide Festival",
            //    //    Description = "Excite your senses at one of the world’s most innovative festivals, the biennial Adelaide Festival with dance, theatre, and art.",
            //    //    City = "Adelaide",
            //    //    StartDate = new DateTime(2014,2,28),
            //    //    EndDate = new DateTime(2014,3,16),
            //    //    State = AustralianStateEnum.SA,
            //    //    Accomodation = "2 nights at The Hilton, Adelaide",
            //    //    Price = 400.00m,
            //    //    ImageUrl = @"../../Images/Packages/AdelaideFestival.jpg",
            //    //    ApplicationUserId = ApplicationUserId2,
            //    //    Status = PackageStatus.Available },

            //    //new Activity {
            //    //    ActivityId = 8,
            //    //    Name = "Clipsal 500",
            //    //    Description = "Feel the adrenalin rush during four days of street parties, live entertainment and extreme motor sport action.",
            //    //    City = "Adelaide",
            //    //    StartDate = new DateTime(2014,2,27),
            //    //    EndDate = new DateTime(2014,3,22),
            //    //    State = AustralianStateEnum.SA,
            //    //    Accomodation = "4 nights at the Mercure Grosvenor Hotel, Adelaide",
            //    //    Price = 500.00m,
            //    //    ImageUrl = @"../../Images/Packages/Clipsal500.jpg",
            //    //    ApplicationUserId = ApplicationUserId2,
            //    //    Status = PackageStatus.Available },

            //    new Activity {
            //        ActivityId = 9,
            //        Name = "The Horizontal Falls",
            //        Description = "Witness a spectacular phenomena of the Kimberley coast in Western Australia as seawater rushes through the narrow gorges.",
            //        City = "Talbot Bay",
            //        State = AustralianStateEnum.WA,
            //        Accomodation = "3 nights at the McLarty Ranges Inn, Talbot Bay",
            //        Price = 400.00m,
            //        ImageUrl = @"../../Images/Packages/TalbotBay.jpg",
            //        ApplicationUserId = ApplicationUserId1,
            //        Status = PackageStatus.Available },

            //    //new Activity {
            //    //    ActivityId = 10,
            //    //    Name = "Sydney Royal Easter Show",
            //    //    Description = "Enjoy Australia’s agricultural heritage with great food, wine and carnival rides for the whole family.",
            //    //    City = "Sydney",
            //    //    StartDate = new DateTime(2014,4,10),
            //    //    EndDate = new DateTime(2014,4,23),
            //    //    State = AustralianStateEnum.NSW,
            //    //    Accomodation = "1 night at the Sebel Townhouse, Sydney",
            //    //    Price = 400.00m,
            //    //    ImageUrl = @"../../Images/Packages/RoyalEasterShow.jpg",
            //    //    ApplicationUserId = ApplicationUserId1,
            //    //    Status = PackageStatus.Available },
            //};

            //context.Activities.AddOrUpdate(c => c.Description, activities);
            //context.SaveChanges();

            //#endregion

            #region Packages

            List<Package> packageList = new List<Package>();

            for (int i = 0; i < 10; i++)
            {
                packageList.Add(new Package
                {
                    Name = "Sydney Package" + i,
                    City = "Sydney",
                    State = AustralianStateEnum.NSW,
                    Accomodation = "2 nights at the Grace Hotel, Sydney",
                    Price = 400.00m,
                    ImageUrl = @"../../Images/Packages/OperaOnSydneyHarbour.jpg",
                    ApplicationUserId = ApplicationUserId4,
                    Status = PackageStatusEnum.Available,

                    Activities = new List<Activity> {
                        new Activity {
                            Status = PackageStatusEnum.Available,
                            Name = "Opera on Sydney Harbour" + i,
                            Address = "Sample Address",
                            Description = "Indulge your love of opera performed on a shimmering stage on the waters of Sydney Harbour.",
                        },

                        new Activity {
                            Status = PackageStatusEnum.Discontinued,
                            Name = "Sydney Royal Easter Show" + i,
                            Address = "Sample Address",
                            Description = "Enjoy Australia’s agricultural heritage with great food, wine and carnival rides for the whole family.",
                        }
                    }
                });

                packageList.Add(new Package
                {
                    Name = "Adelaide Package" + i,
                    City = "Adelaide",
                    State = AustralianStateEnum.SA,
                    Accomodation = "4 nights at the Mercure Grosvenor Hotel, Adelaide",
                    Price = 900.00m,
                    ImageUrl = @"../../Images/Packages/AdelaideFestival.jpg",
                    ApplicationUserId = ApplicationUserId4,
                    Status = PackageStatusEnum.Available,

                    Activities = new List<Activity> {
                        new Activity {
                            Status = PackageStatusEnum.Available,
                            Name = "Clipsal 500" + i,
                            Address = "Sample Address",
                            Description = "Feel the adrenalin rush during four days of street parties, live entertainment and extreme motor sport action.",
                         },

                        new Activity {
                            Status = PackageStatusEnum.Available,
                            Name = "Adelaide Festival" + i,
                            Address = "Sample Address",
                            Description = "Excite your senses at one of the world’s most innovative festivals, the biennial Adelaide Festival with dance, theatre, and art.",
                        },

                         new Activity {
                             Status = PackageStatusEnum.Available,
                             Name = "WOMADelaide" + i,
                             Address = "Sample Address",
                             Description = "Enjoy the world’s best traditional and contemporary musicians, dancers and DJs in this outdoor festival held in Botanic Park in Adelaide.",
                        }
                    }
                });
            }

            Package[] packages = packageList.ToArray<Package>();

            context.Packages.AddOrUpdate(c => c.Name, packages);
            context.SaveChanges();


            #endregion
        }
    }
}
