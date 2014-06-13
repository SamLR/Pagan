using Pagan.DbComponents;
using Pagan.Relationships;

namespace Pagan.Tests.TestControllers
{
    /// <summary>
    /// This test controller leans heavily on configuration conventions.
    /// There is no Schema property, and no configuration method
    /// It should call DbConfig to obtain DefaultSchema, PrimaryKey, and ForeignKey values.
    /// </summary>
    public class Product
    {
        public Table Products { get; set; }

        public Column Id { get; set; }
        public Column Name { get; set; }
        public Column SupplierId { get; set; }

        public HasMany<OrderDetail> OrderDetails { get; set; }
        public WithOne<Supplier> Supplier { get; set; }
    }

    public class Supplier
    {
        public Table Suppliers { get; set; }

        public Column Id { get; set; }
        public Column Name { get; set; }

        public HasMany<Product> Products { get; set; }
    }
}
