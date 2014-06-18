using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Pagan.Adapters;
using Pagan.Queries;
using Pagan.Registry;

namespace Pagan
{
    public class Database : IDisposable
    {

        private readonly IQueryAdapter _adapter;
        private readonly DbConnection _connection;
        private readonly ITableFactory _factory;
        private bool _disposed;

        public Database(string name, ITableConventions conventions = null)
        {
            var item = ConfigurationManager.ConnectionStrings[name];

            var providerName = String.IsNullOrEmpty(item.ProviderName)
                ? Settings.SqlProvider
                : item.ProviderName;

            _adapter = Settings.GetAdapter(providerName);
            _factory = new TableFactory(conventions ?? Settings.GetConventions(name));
            _connection = GetDbConnection(providerName, item.ConnectionString);
            
        }

        private static DbConnection GetDbConnection(string providerName, string connectionString)
        {
            var connection = DbProviderFactories.GetFactory(providerName).CreateConnection();

            if (connection == null)
                throw ConfigurationError.MissingAdoProvider(providerName);

            connection.ConnectionString = connectionString;

            return connection;
        }

        private DbDataReader ExecuteQuery(DbCommand cmd)
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();

            return cmd.ExecuteReader(CommandBehavior.SingleResult);
        }

        public IEnumerable<dynamic> Query<T>(Func<T, Query> action)
        {
            var query = action(_factory.GetTable<T>().Controller);
            var cmd = _adapter.GetCommand(query, _connection);
            var reader = ExecuteQuery(cmd);
            return query.CreateQueryResult().Spool(reader);
        }

        

        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                if (_connection.State != ConnectionState.Closed)
                    _connection.Close();
            }
            finally
            {
                _connection.Dispose();
                _disposed = true;
            }
        }
    }
}