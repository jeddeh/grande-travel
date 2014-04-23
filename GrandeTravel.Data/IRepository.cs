﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Data
{
    public interface IRepository<T> where T : class
    {
        T Get(object primaryKey);

        void Insert(T entity);

        void Update(T entity);

        void Delete(T entity);

        IQueryable<T> Query();
    }
}
