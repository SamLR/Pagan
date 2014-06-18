using System;

namespace Pagan.Registry
{
    public class ConfigurationError: Exception
    {
        
        private ConfigurationError(string errorType, string message, params object[] args) : base(String.Format(message,args))
        {
            ErrorType = errorType;
        }

        public string ErrorType { get; private set; }

        internal static ConfigurationError MissingTable(Type controllerType)
        {
            return new ConfigurationError(
                "MissingTable",
                "No settable property of type Pagan.DbComponents.Table was defined on the Table class {0}", 
                controllerType);
        }

        internal static ConfigurationError MissingColumns(Type controllerType)
        {
            return new ConfigurationError(
                "MissingColumns",
                "No settable properties of type Pagan.DbComponents.Column were defined on the Table class {0}",
                controllerType);
        }

        internal static ConfigurationError MissingKey(Type controllerType)
        {
            return new ConfigurationError(
                "MissingKey",
                "No primary key was explicitly defined or could be inferred on the Table class {0}",
                controllerType);
        }

        internal static ConfigurationError MissingForeignKey(Type controllerType)
        {
            return new ConfigurationError(
                "MissingForeignKey",
                "No foreign key was explicitly defined or could be inferred on the Table class {0}",
                controllerType);
        }

        internal static ConfigurationError MissingAdoProvider(string providerName)
        {
            return new ConfigurationError(
                "MissingADOProvider",
                "No ADO provider type was registered for the provider with name {0}",
                providerName);

        }

        internal static ConfigurationError MissingDbAdapter(string providerName)
        {
            return new ConfigurationError(
                "MissingDbAdapter",
                "No Pagan Db Adapter was registered for the provider with name {0}",
                providerName);
        }
    }
}
