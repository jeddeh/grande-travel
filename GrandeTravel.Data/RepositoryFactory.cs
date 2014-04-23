using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Data.Implementation.EntityFramework;

namespace GrandeTravel.Data
{
    public static class RepositoryFactory
    {
        public static IRepository<T> GetRepository<T>(IUnitOfWork unitOfWork) where T : class
        {
            // Return an instance of the Entity Framework repository
            return new Repository<T>(unitOfWork);
        }

        public static IUnitOfWork GetUnitOfWork(string connectionOrConfig)
        {
            return new UnitOfWork(connectionOrConfig);
        }
    }
}