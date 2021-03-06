﻿using System;
using System.Linq;

namespace Models.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetPage(int? page, int? pageSize); 
        T FindById(int id);
        void Insert(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
    }
}
