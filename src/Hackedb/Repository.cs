using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Hackedb.Contracts;

namespace Hackedb
{
    public abstract class Repository<TEntity> : IAsyncDisposable
    {
        private readonly IDbChannel _dbChannel;

        protected Repository(IDbChannel dbChannel)
        {
            _dbChannel = dbChannel;
        }

        protected abstract TEntity DefaultMap(IDataRecord @record);

        protected async Task Insert(string query, params object[] values)
        {
            await Execute($"INSERT {query.TrimStart()}", values);
        }

        protected async Task Update(string query, params object[] values)
        {
            await Execute($"UPDATE {query.TrimStart()}", values);
        }

        protected async Task Delete(string query, params object[] values)
        {
            await Execute($"DELETE {query.TrimStart()}", values);
        }

        private async Task Execute(string query, IEnumerable<object> values)
        {
            await _dbChannel.ExecuteNonQuery(query, values);
        }

        protected async Task<IList<TEntity>> Select(string query,
            IEnumerable<object>? values = null, Func<IDataRecord, TEntity>? mapMethod = null)
        {
            return await _dbChannel.ExecuteReader($"SELECT {query.TrimStart()}",
                mapMethod ?? DefaultMap, values);
        }

        public async ValueTask DisposeAsync()
        {
            await _dbChannel.Close();
            GC.SuppressFinalize(this);
        }
    }
}
