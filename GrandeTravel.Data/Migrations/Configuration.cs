namespace GrandeTravel.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using GrandeTravel.Entity;
    using GrandeTravel.Entity.Enums;

    using WebMatrix.Data;
    using WebMatrix.WebData;
    using System.Web.Security;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Diagnostics;

    internal sealed class Configuration : DbMigrationsConfiguration<GrandeTravel.Data.StorageContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GrandeTravel.Data.StorageContext context)
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "Email", true);
            var roles = (SimpleRoleProvider)Roles.Provider;

            #region roles

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

            TravelUser[] admin = new TravelUser[]
            {
                new TravelUser {
                    TravelUserId = adminId,
                     FirstName = "Claire",
                     LastName = "Rivera",
                     Address = "201 Walker Street",
                     City = "Townsville",
                     Postcode = "4810",
                     State = AustralianStateEnum.QLD,
                    Phone = "0401 605 651",
                    Email = adminEmail }
            };

            context.TravelUsers.AddOrUpdate(admin);
            context.SaveChanges();

            Roles.AddUserToRole(adminEmail, "Admin");

            #endregion

            #region TravelUsers

            string email1 = "customer1@sample.com";
            WebSecurity.CreateUserAndAccount(email1, "111111");
            int travelUserId1 = WebSecurity.GetUserId(email1);

            string email2 = "customer2@sample.com";
            WebSecurity.CreateUserAndAccount(email2, "111111");
            int travelUserId2 = WebSecurity.GetUserId(email2);

            string email3 = "customer3@sample.com";
            WebSecurity.CreateUserAndAccount(email3, "111111");
            int travelUserId3 = WebSecurity.GetUserId(email3);

            string email4 = "provider1@sample.com";
            WebSecurity.CreateUserAndAccount(email4, "111111");
            int travelUserId4 = WebSecurity.GetUserId(email4);

            string email5 = "provider2@sample.com";
            WebSecurity.CreateUserAndAccount(email5, "111111");
            int travelUserId5 = WebSecurity.GetUserId(email5);

            string email6 = "provider3@sample.com";
            WebSecurity.CreateUserAndAccount(email6, "111111");
            int travelUserId6 = WebSecurity.GetUserId(email6);

            TravelUser[] travelUsers = new TravelUser[]
            {
                new TravelUser {
                    TravelUserId = travelUserId1,
                     FirstName = "John",
                     LastName = "Stanton",
                     Address = "5 Short Street, Petersham",
                     City = "Sydney",
                     Postcode = "2049",
                     State = AustralianStateEnum.NSW,
                     Phone = "0401 605 650",
                     Email = email1 },

                new TravelUser { 
                    TravelUserId = travelUserId2,
                    FirstName = "Bridget",
                    LastName = "Jones",
                    Address = "6/10 Smith Street",
                    City = "Euroa",
                    Postcode = "3666",
                    State = AustralianStateEnum.VIC,
                    Phone = "0403 605 650",
                    Email = email2 },

                new TravelUser { 
                    TravelUserId = travelUserId3,
                    FirstName = "Chris",
                    LastName = "Nguyen",
                    Address = "101 Harry's Place",
                    City = "Hobart",
                    Postcode = "7001",
                    State = AustralianStateEnum.TAS,
                    Phone = "0401 605 651",
                    Email = email3 },

               new TravelUser {
                    TravelUserId = travelUserId4,
                    FirstName = "Peter",
                    LastName = "Montgomery",
                    Address = "158 Walker Street",
                    City = "Townsville",
                    Postcode = "4810",
                    State = AustralianStateEnum.QLD,
                    Phone = "0403 605 650",
                    Email = email4 },

                new TravelUser { 
                    TravelUserId = travelUserId5,
                    FirstName = "Julius",
                    LastName = "Dutton",
                    Address = "158 Georges Terrace",
                    City = "Oatlands Park",
                    Postcode = "5046",
                    State = AustralianStateEnum.SA,
                    Phone = "0403 605 650",
                    Email = email5 },

                new TravelUser { 
                    TravelUserId = travelUserId6,
                    FirstName = "Mary",
                    LastName = "Poulson",
                    Address = "101 Jareys Close",
                    City = "Hobart",
                    Postcode = "7001",
                    State = AustralianStateEnum.TAS,
                    Phone = "0401 605 651",
                    Email = email6 },
            };

            try
            {
                context.TravelUsers.AddOrUpdate(travelUsers);
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

            Roles.AddUsersToRole(new string[] { email1, email2, email3 }, "Customer");
            Roles.AddUsersToRole(new string[] { email4, email5, email6 }, "Provider");

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
            //    //    ImageUrl = @"../../Images/Package/OperaOnSydneyHarbour.jpg",
            //    //    TravelUserId = travelUserId1,
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
            //        ImageUrl = @"../../Images/Package/MelbourneFoodAndWine.jpg",
            //        TravelUserId = travelUserId1,
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
            //        ImageUrl = @"../../Images/Package/Ballooning.jpg",
            //        TravelUserId = travelUserId2,
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
            //    //    ImageUrl = @"../../Images/Package/Womadelaide.jpg",
            //    //    TravelUserId = travelUserId2,
            //    //    Status = PackageStatus.Available },

            //    new Activity {
            //        ActivityId = 5,
            //        Name = "Formula 1 Australian Grand Prix",
            //        Description = "Watch the world’s fastest cars compete in this thrilling Formula 1 race in Melbourne at the Albert Park circuit.",
            //        City = "Melbourne",
            //        State = AustralianStateEnum.VIC,
            //        Accomodation = "3 nights at the Swanston Hotel, Melbourne",
            //        Price = 400.00m,
            //        ImageUrl = @"../../Images/Package/AustralianGrandPrix.jpg",
            //        TravelUserId = travelUserId1,
            //        Status = PackageStatus.Discontinued },

            //    new Activity {
            //        ActivityId = 6,
            //        Name = "Sculptures by the Sea",
            //        Description = "The stunning Cottesloe Beach will be transformed into a spectacular sculpture park, featuring more than 70 artists from Australia and across the world.",
            //        City = "Cottesloe Beach",
            //        State = AustralianStateEnum.WA,
            //        Accomodation = "3 nights at the Mosman Park, Cottesloe",
            //        Price = 650.00m,
            //        ImageUrl = @"../../Images/Package/SculpturesByTheSea.jpg",
            //        TravelUserId = travelUserId1,
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
            //    //    ImageUrl = @"../../Images/Package/AdelaideFestival.jpg",
            //    //    TravelUserId = travelUserId2,
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
            //    //    ImageUrl = @"../../Images/Package/Clipsal500.jpg",
            //    //    TravelUserId = travelUserId2,
            //    //    Status = PackageStatus.Available },

            //    new Activity {
            //        ActivityId = 9,
            //        Name = "The Horizontal Falls",
            //        Description = "Witness a spectacular phenomena of the Kimberley coast in Western Australia as seawater rushes through the narrow gorges.",
            //        City = "Talbot Bay",
            //        State = AustralianStateEnum.WA,
            //        Accomodation = "3 nights at the McLarty Ranges Inn, Talbot Bay",
            //        Price = 400.00m,
            //        ImageUrl = @"../../Images/Package/TalbotBay.jpg",
            //        TravelUserId = travelUserId1,
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
            //    //    ImageUrl = @"../../Images/Package/RoyalEasterShow.jpg",
            //    //    TravelUserId = travelUserId1,
            //    //    Status = PackageStatus.Available },
            //};

            //context.Activities.AddOrUpdate(c => c.Description, activities);
            //context.SaveChanges();

            //#endregion

            #region Packages

            Package[] packages = new Package[] {
                new Package {
                    Name = "Sydney Package",
                    City = "Sydney",
                    State = AustralianStateEnum.NSW,
                    Accomodation = "2 nights at the Grace Hotel, Sydney",
                    Price = 400.00m,
                    ImageUrl = @"../../Images/Package/OperaOnSydneyHarbour.jpg",
                    TravelUserId = travelUserId4,
                    Status = PackageStatus.Available,

                    Activities = new List<Activity> {
                        new Activity {
                            Name = "Opera on Sydney Harbour",
                            Description = "Indulge your love of opera performed on a shimmering stage on the waters of Sydney Harbour.",
                            //ImageUrl = @"../../Images/Package/OperaOnSydneyHarbour.jpg",
                        },

                        new Activity {
                            Name = "Sydney Royal Easter Show",
                            Description = "Enjoy Australia’s agricultural heritage with great food, wine and carnival rides for the whole family.",
                            //ImageUrl = @"../../Images/Package/RoyalEasterShow.jpg",
                        }
                    }
                },

                new Package {
                    Name = "Adelaide Package",
                    City = "Adelaide",
                    State = AustralianStateEnum.SA,
                    Accomodation = "4 nights at the Mercure Grosvenor Hotel, Adelaide",
                    Price = 900.00m,
                    ImageUrl = @"../../Images/Package/OperaOnSydneyHarbour.jpg",
                    TravelUserId = travelUserId5,
                    Status = PackageStatus.Available,

                    Activities = new List<Activity> {
                        new Activity {
                            Name = "Clipsal 500",
                            Description = "Feel the adrenalin rush during four days of street parties, live entertainment and extreme motor sport action.",
                            //ImageUrl = @"../../Images/Package/Clipsal500.jpg"
                         },

                        new Activity {
                            Name = "Adelaide Festival",
                            Description = "Excite your senses at one of the world’s most innovative festivals, the biennial Adelaide Festival with dance, theatre, and art.",
                            //ImageUrl = @"../../Images/Package/AdelaideFestival.jpg"
                        },

                         new Activity {
                             Name = "WOMADelaide",
                            Description = "Enjoy the world’s best traditional and contemporary musicians, dancers and DJs in this outdoor festival held in Botanic Park in Adelaide.",
                            //ImageUrl = @"../../Images/Package/Womadelaide.jpg"
                        }
                    }
                }
            };

            context.Packages.AddOrUpdate(c => c.Name, packages);
            context.SaveChanges();


            #endregion
        }
    }
}
