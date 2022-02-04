using System.Collections.Generic;

namespace Hackedb.Contracts
{
    public interface IRepository<TEntity>
    {
        void Add(TEntity entity);
        IList<TEntity> GetAll();
    }
}
