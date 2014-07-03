using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using Pagan.Adapters;
using Pagan.Commands;
using Pagan.Queries;
using Pagan.Registry;
using Pagan.Results;
using CommandType = System.Data.CommandType;

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

        public DbTransaction Transaction()
        {
            EnsureConnection();
            return _connection.BeginTransaction();
        }

        public IEnumerable<dynamic> Query<T>(Func<T, Query> action, DbCommand dbCommand = null)
        {
            var query = GetPaganQuery(action);
            using (var cmd = dbCommand ?? GetDbCommand(query))
            {
                var reader = ExecuteReader(cmd);
                return query.CreateEntitySet().Spool(reader).ToArray();
            }
        }

        public int Command<T>(Func<T, Command> action, DbCommand dbCommand = null)
        {
            var paganCmd = GetPaganCommand(action);
            using (var cmd = dbCommand ?? GetDbCommand(paganCmd))
            {
                return ExecuteNonQuery(cmd);
            }
        }

        public int Sql(string sql, bool isSproc, object args = null)
        {
            using (var cmd = GetDbCommand(sql, isSproc, args))
            {
                return ExecuteNonQuery(cmd);
            }
        }

        public IEnumerable<dynamic> SqlWithResults(string sql, bool isSproc, object args = null)
        {
            using (var cmd = GetDbCommand(sql, isSproc, args))
            {
                return ReaderConverter.ToDynamic(ExecuteReader(cmd)).ToArray();
            }
        }

        public object SqlWithScalar(string sql, bool isSproc, object args = null)
        {
            using (var cmd = GetDbCommand(sql, isSproc, args))
            {
                return ExecuteScalar(cmd);
            }
        }

        public DbCommand GetDbCommand(string cmdText, bool isSproc, object args)
        {
            if (cmdText == null) throw new ArgumentNullException("cmdText");

            var cmd = _connection.CreateCommand();
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = 0;
            cmd.CommandType = isSproc ? CommandType.StoredProcedure : CommandType.Text;

            if (args != null)
                args.GetType().GetProperties().ForEach(p =>
                {
                    var name = p.Name.StartsWith("@") || p.Name.StartsWith("_")
                        ? p.Name.Substring(1)
                        : p.Name;

                    var direction = p.Name.StartsWith("@")
                        ? ParameterDirection.Output
                        : p.Name.StartsWith("_")
                            ? ParameterDirection.InputOutput
                            : ParameterDirection.Input;

                    if (String.Equals("return_value", name, StringComparison.InvariantCultureIgnoreCase) &&
                        direction == ParameterDirection.Output)
                    {
                        direction = ParameterDirection.ReturnValue;
                        name = "RETURN_VALUE";
                    }

                    var dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = "@" + name;
                    dbParameter.Value = p.GetValue(args, null);
                    dbParameter.Direction = direction;
                    cmd.Parameters.Add(dbParameter);
                });

            return cmd;
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

        internal Query GetPaganQuery<T>(Func<T, Query> action)
        {
            return action(_factory.GetTable<T>().Controller);
        }

        internal Command GetPaganCommand<T>(Func<T, Command> action)
        {
            return action(_factory.GetTable<T>().Controller);
        }

        internal DbCommand GetDbCommand(IQuery query)
        {
            var cmd = _connection.CreateCommand();
            var translation = _adapter.TranslateQuery(query);
            translation.BuildCommand(cmd);
            return cmd;
        }

        internal DbCommand GetDbCommand(ICommand cmd)
        {
            var dbCommand = _connection.CreateCommand();
            var translation = _adapter.TranslateCommand(cmd);
            translation.BuildCommand(dbCommand);
            return dbCommand;
        }
    }
}