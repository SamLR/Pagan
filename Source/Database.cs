using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
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
        private DbTransaction _transaction;
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

        private DbCommand GetDbCommand(IDbTranslation translation)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;
            cmd.CommandText = translation.GetCommandText();
#if DEBUG
            Debug.WriteLine(cmd.CommandText);
#endif

            translation.Parameters.ForEach(x =>
            {
#if DEBUG
                Debug.WriteLine(x.Key + ": " + x.Value);
#endif
                var p = cmd.CreateParameter();
                p.ParameterName = x.Key;
                p.Value = x.Value;
                p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p);
            });

            return cmd;
        }

        internal DbCommand GetDbCommand<T>(Func<T, Command> action)
        {
            var cmd = action(_factory.GetTable<T>().Controller);
            var translation = _adapter.TranslateCommand(cmd);
            return GetDbCommand(translation);
        }
        
        internal DbCommand GetDbCommand<T>(Func<T, Query> action)
        {
            var cmd = action(_factory.GetTable<T>().Controller);
            var translation = _adapter.TranslateQuery(cmd);
            return GetDbCommand(translation);
        }

        private DbDataReader ExecuteQuery(DbCommand cmd)
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();

            return cmd.ExecuteReader(CommandBehavior.SingleResult);
        }

        private int ExecuteCommand(DbCommand cmd)
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();

            _transaction = _connection.BeginTransaction();

            return cmd.ExecuteNonQuery();
        }

        public IEnumerable<dynamic> Execute<T>(Func<T, Query> action)
        {
            var query = action(_factory.GetTable<T>().Controller);
            var translation = _adapter.TranslateQuery(query);
            var cmd = GetDbCommand(translation);
            var reader = ExecuteQuery(cmd);
            return query.CreateEntitySet().Spool(reader);
        }

        public void Execute<T>(Func<T, Command> action)
        {
            var paganCmd = action(_factory.GetTable<T>().Controller);
            var translation = _adapter.TranslateCommand(paganCmd);
            var dbCmd = GetDbCommand(translation);
            ExecuteCommand(dbCmd);
            _transaction.Commit();
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
                if (_transaction != null) _transaction.Dispose();
                _connection.Dispose();
                _disposed = true;
            }
        }
    }
}