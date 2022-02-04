using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace Hackedb.Extensions
{
    public static class DbCommandExtensions
    {
        public static void MapPlaceHolders(this DbCommand command, IEnumerable<object>? values)
        {
            if (values == null) return;
            List<object> valuesList = values.ToList();
            command.MapCommandParameters(GeneratePlaceHolders(valuesList), valuesList);
        }

        private static IEnumerable<string> GeneratePlaceHolders(IEnumerable<object> values)
        {
            return values.Select((_, index) => $"@{index}");
        }

        private static void MapCommandParameters(this DbCommand command,
            IEnumerable<string> placeHolders, IEnumerable<object> values)
        {
            placeHolders.ForEach(values, command.AddDbParameter);
        }

        private static void AddDbParameter(this DbCommand command, string placeHolder, object value)
        {
            command.Parameters.Add(command.CreateDbParameter(placeHolder, value));
        }

        private static DbParameter CreateDbParameter(this DbCommand command, string placeHolder,
            object value)
        {
            DbParameter dbParameter = command.CreateParameter();
            dbParameter.ParameterName = placeHolder;
            dbParameter.Value         = value;
            return dbParameter;
        }

        public static async Task<IList<TEntity>> ToList<TEntity>(this DbCommand command,
            Func<IDataRecord, TEntity> mapMethod)
        {
            IList<TEntity> result = new List<TEntity>();

            await using DbDataReader dbDataReader = await command.ExecuteReaderAsync();
            while (await dbDataReader.ReadAsync())
            {
                try
                {
                    result.Add(mapMethod(dbDataReader));
                }
                catch (SqlNullValueException)
                {
                    // ignore
                }
            }

            return result;
        }
    }
}
