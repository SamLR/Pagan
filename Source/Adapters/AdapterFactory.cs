using System;

namespace Pagan.Adapters
{
    class AdapterFactory
    {
        internal enum Server
        {
            MSSql2008,
            MSSql2012,
            Oracle,
            MySql,
            PostGreSql
        }

        public ISqlAdapter GetAdapter(Server server)
        {
            switch (server)
            {
                case Server.MSSql2008:
                case Server.MSSql2012:
                    return new MSSqlServerAdapter();

                default:
                    throw new Exception(String.Format("Database adapter not implemented for '{0}'", server));
            }
        }
        
    }
}