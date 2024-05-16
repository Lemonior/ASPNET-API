using ASPNET_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_API.Data.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        int Delete(T entity);
    }
}
