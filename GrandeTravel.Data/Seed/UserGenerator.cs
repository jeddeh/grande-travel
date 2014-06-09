using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Data.Seed
{
    public static class UserGenerator
    {
        // Generate a list of fake users
        public static List<FakeUserWithPassword> GenerateUsers(string file, int numberOfUsers)
        {
            List<FakeUserWithPassword> users = new List<FakeUserWithPassword>();

            using (StreamReader reader = new StreamReader(File.OpenRead(Directory.GetCurrentDirectory() + file)))
            {
                for (int i = 0; i < numberOfUsers; i++)
                {
                    if (reader.EndOfStream)
                    {
                        break;
                    }

                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    ApplicationUser applicationUser = new ApplicationUser
                    {
                        FirstName = values[0],
                        LastName = values[1],
                        Address = values[2],
                        City = values[3],
                        State = (AustralianStateEnum)Enum.Parse(typeof(AustralianStateEnum), values[4]),
                        Postcode = values[5],
                        Phone = values[6],
                        Email = values[7],
                    };

                    FakeUserWithPassword fakeUser = new FakeUserWithPassword();
                    fakeUser.User = applicationUser;
                    fakeUser.Password = values[8];
                    users.Add(fakeUser);
                }
            }

            return users;
        }
    }
}
