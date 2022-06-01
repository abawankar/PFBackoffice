using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public abstract class RepositoryModel<T>
    {
        public abstract T GetById(int id);
        public abstract IList<T> GetAll();
        public abstract void Edit(T obj);
        public abstract void Insert(T obj);
        public abstract bool Delete(int id);
    }
}