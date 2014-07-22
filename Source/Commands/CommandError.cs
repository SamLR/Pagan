using System;

namespace Pagan.Commands
{
    public class CommandError : Exception
    {
        private CommandError(string errorType, string message, params object[] args): base(String.Format(message, args))
        {
            ErrorType = errorType;
        }

        public string ErrorType { get; private set; }

        internal static CommandError EmptyColumnValues(CommandType commandType, Table table)
        {
            return new CommandError(
                "EmptyColumnValues",
                "Pagan command of type {0) on table {1} requires one or more column values",
                commandType,
                table);
        }

        internal static CommandError AttemptToAlterKeyValues(Table table)
        {
            return new CommandError(
                "AttemptToAlterKeyValues",
                "A Pagan update command attempted to alter a primary key value for table '{0)'",
                table);
        }

        internal static CommandError InvalidNumberOfKeyValues(Table table, int expected, int actual)
        {
            return new CommandError(
                "InvalidNumberOfKeyValues",
                "A Pagan insert command for table '{0)' requires {1} key values, but received {2}",
                table,
                expected,
                actual);
        }

        internal static CommandError FilterInsertError(Table table)
        {
            return new CommandError(
                "FilterInsertError",
                "A Pagan insert command for table '{0)' attempted to add a filter predicate which is forbidden",
                table);
        }

    }
}