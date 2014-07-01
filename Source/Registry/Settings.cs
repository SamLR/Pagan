using System;
using System.Collections.Generic;
using Pagan.Adapters;

namespace Pagan.Registry
{
    public class Settings
    {
        public const string SqlProvider = "System.Data.SqlClient";

        private static readonly Dictionary<string, IQueryAdapter> Adapters;
        private static readonly Dictionary<string, ITableConventions> Conventions;

        static Settings()
        {
            Adapters = new Dictionary<string, IQueryAdapter>
            {
                {SqlProvider.ToLowerInvariant(), new SqlQueryAdapter()}
            };

            Conventions = new Dictionary<string, ITableConventions>
            {
                {String.Empty, new TableConventions()}
            };
        }

        public static void RegisterQueryAdapter(string provider, IQueryAdapter adapter)
        {
            if (provider == null) throw new ArgumentNullException("provider");

            Adapters[provider.ToLowerInvariant()] = adapter;
        }

        public static IQueryAdapter GetAdapter(string provider)
        {
            if (provider == null) throw new ArgumentNullException("provider");

            IQueryAdapter adapter;

            if (!Adapters.TryGetValue(provider.ToLowerInvariant(), out adapter))
                throw ConfigurationError.MissingDbAdapter(provider);
            
            return adapter;
        }

        public static void RegisterTableConventions(string database, ITableConventions conventions)
        {
            if (String.IsNullOrEmpty(database)) throw new ArgumentNullException("database");

            Conventions[database.ToLowerInvariant()] = conventions;
        }

        public static ITableConventions GetConventions(string database)
        {
            ITableConventions conventions;

            if (!Conventions.TryGetValue(database.ToLowerInvariant(), out conventions))
                conventions = Conventions[String.Empty];

            return conventions;
        }
    }
}
