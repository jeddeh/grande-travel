using GrandeTravel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Data.Seed
{
    // Hack in place of a derived class from ApplicationUser to keep Entity Framework happy.
    public struct FakeUserWithPassword
    {
       public ApplicationUser User { get; set; }
       public string Password { get; set; }
    }
}
