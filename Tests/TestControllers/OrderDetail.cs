using Pagan.Relationships;

namespace Pagan.Tests.TestControllers
{
    /// <summary>
    /// This test Table provides it's own configuration rather than relying on configuration conventions.
    /// So it provides a Configure method to sets its primary keys and foreign keys, and defined a Schema property.
    /// It should NOT cause DefaultSchema, PrimaryKey, or ForeignKey helpers to be called on the DbConfig.
    /// </summary>
    public class OrderDetail
    {
        public void Configure()
        {
            OrderDetails.SetKey(OrderId, ProductId);
            Cost.DbName = "StandardCost";
            Product.SetForeignKey(ProductId);

            ConfigureWasCalled = true;
        }

        public Table OrderDetails { get; set; }
        public Schema Dbo { get; set; }
        public Column OrderId { get; set; }
        public Column ProductId { get; set; }
        public Column Quantity { get; set; }
        public Column Cost { get; set; }
        public WithOne<Product> Product { get; set; }
        public WithOne<Order> Order { get; set; }

        internal bool ConfigureWasCalled;
    }
}