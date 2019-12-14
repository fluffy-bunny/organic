using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDB.SQLRepo.Contract
{
    public interface IRepository<T>
    {
        Task<T> Insert(T item);

        Task<List<T>> Get(Expression<Func<T, bool>> func);

        Task<T> Update(T item);

        Task Delete(string uniqueId);

    }
}
