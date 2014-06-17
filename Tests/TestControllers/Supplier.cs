using Pagan.Relationships;

namespace Pagan.Tests.TestControllers
{
    public class Supplier
    {
        public Table Suppliers { get; set; }

        public Column Id { get; set; }
        public Column Name { get; set; }

        public HasMany<Product> Products { get; set; }
    }
}