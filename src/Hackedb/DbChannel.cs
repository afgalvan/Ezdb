using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Hackedb.Contracts;
using Hackedb.Extensions;

namespace Hackedb
{
    public class DbChannel : IDbChannel
    {
        private readonly DbConnection _dbConnection;

        public DbChannel(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task ExecuteNonQuery(string query, IEnumerable<object> values)
        {
            await _dbConnection.OpenAsync();
            await using DbCommand command = CreateCommand(query, values);
            await command.ExecuteNonQueryAsync();
            await _dbConnection.CloseAsync();
        }

        public async Task<IList<TEntity>> ExecuteReader<TEntity>(string query,
            Func<IDataRecord, TEntity> mapMethod, IEnumerable<object>? values = null)
        {
            await _dbConnection.OpenAsync();
            await using DbCommand command  = CreateCommand(query, values);
            IList<TEntity>        entities = await command.ToList(mapMethod);
            await _dbConnection.CloseAsync();
            return entities;
        }

        private DbCommand CreateCommand(string query, IEnumerable<object>? values)
        {
            DbCommand command = _dbConnection.CreateCommand();
            command.CommandText = query;
            command.MapPlaceHolders(values);
            return command;
        }

        public async Task Close()
        {
            await _dbConnection.CloseAsync();
        }
    }
}
