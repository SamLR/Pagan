using System;

namespace Pagan.Queries
{
    public class ExecutionError: Exception
    {
        private ExecutionError(string errorType, string message, params object[] args)
            : base(String.Format(message, args))
        {
            ErrorType = errorType;
        }

        public string ErrorType { get; private set; }

        internal static ExecutionError PartiallyNullKey(Table table)
        {
            return new ExecutionError(
                "PartiallyNullKey",
                "This shouldn't happen, but one of the key columns in table {0} came back as NULL", 
                table);
        }
    }
}
