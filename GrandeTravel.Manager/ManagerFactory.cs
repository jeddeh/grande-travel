using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Data;
using GrandeTravel.Entity;
using GrandeTravel.Manager.Implementation;

namespace GrandeTravel.Manager
{
    public static class ManagerFactory
    {
        public static IManager<T> GetManager<T>(IRepository<T> repository) where T : class
        {
            return new Manager<T>(repository);
        }
    }
}
