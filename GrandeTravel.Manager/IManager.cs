﻿using System;
using System.Collections.Generic;

namespace GrandeTravel.Manager
{
    // Generic Manager interface
    public interface IManager<T>
    {
        T Create(T item);
        IEnumerable<T> Get(Func<T, bool> predicate);
        IEnumerable<T> EagerGet(Func<T, bool> predicate, string children);
        T GetById(object id);
        void Update(T item);
        void Delete(T item);
    }
}

