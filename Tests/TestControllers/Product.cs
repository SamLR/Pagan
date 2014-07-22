using Pagan.Registry;
using Pagan.Relationships;

namespace Pagan.Tests.TestControllers
{
    public class Product
    {
        public Table Products { get; set; }

        public Column Id { get; set; }
        public Column Name { get; set; }
        public Column SupplierId { get; set; }

        public HasMany<OrderDetail> OrderDetails { get; set; }
        public WithOne<Supplier> Supplier { get; set; }
    }
}
