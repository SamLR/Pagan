using Pagan.SqlObjects;

namespace Pagan.Tests.SampleDefinitions
{
    /// <summary>
    /// Demonstrates a configuration error. 
    /// Trying to create the definition fails because Pagan requires at least one public settable property
    /// of type Key.
    /// </summary>
    public class NoKeyExample
    {
        public Field Name { get; set; }
    }
}