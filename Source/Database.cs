using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Pagan.Adapters;
using Pagan.Commands;
using Pagan.Queries;
using Pagan.Registry;

namespace Pagan
{
    public class Database : IDisposable
    {
        private readonly IDbAdapter _adapter;
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

        private void EnsureConnection()
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
        }

        private DbDataReader ExecuteReader(DbCommand cmd)
        {
            EnsureConnection();
            return cmd.ExecuteReader(CommandBehavior.SingleResult);
        }

        private int ExecuteNonQuery(DbCommand cmd)
        {
            EnsureConnection();
            return cmd.ExecuteNonQuery();
        }

        private object ExecuteScalar(DbCommand cmd)
        {
            EnsureConnection();
            return cmd.ExecuteScalar();
        }

        private DbCommand GetDbCommand(IQuery query)
        {
            var cmd = _connection.CreateCommand();
            var translation = _adapter.TranslateQuery(query);
            translation.BuildCommand(cmd);
            return cmd;
        }

        private DbCommand GetDbCommand(ICommand cmd)
        {
            var dbCommand = _connection.CreateCommand();
            var translation = _adapter.TranslateCommand(cmd);
            translation.BuildCommand(dbCommand);
            return dbCommand;
        }

        public DbTransaction Transaction()
        {
            EnsureConnection();
            return _connection.BeginTransaction();
        }

        public IEnumerable<dynamic> Execute<T>(Func<T, Query> action)
        {
            var query = action(_factory.GetTable<T>().Controller);
            var cmd = GetDbCommand(query);
            var reader = ExecuteReader(cmd);
            return query.CreateEntitySet().Spool(reader);
        }

        public int Execute<T>(Func<T, Command> action)
        {
            var paganCmd = action(_factory.GetTable<T>().Controller);
            var dbCmd = GetDbCommand(paganCmd);
            return ExecuteNonQuery(dbCmd);
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


        internal DbCommand GetDbCommand<T>(Func<T, Command> action)
        {
            var cmd = action(_factory.GetTable<T>().Controller);
            return GetDbCommand(cmd);
        }

        internal DbCommand GetDbCommand<T>(Func<T, Query> action)
        {
            var qry = action(_factory.GetTable<T>().Controller);
            return GetDbCommand(qry);
        }
    }
}