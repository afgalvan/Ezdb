using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Hackedb.Contracts
{
    public interface IDbChannel
    {
        Task ExecuteNonQuery(string query, IEnumerable<object> values);

        Task<IList<TEntity>> ExecuteReader<TEntity>(string query,
            Func<IDataRecord, TEntity> mapMethod, IEnumerable<object>? values = null);

        Task Close();
    }
}
