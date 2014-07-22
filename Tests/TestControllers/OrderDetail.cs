using Pagan.Registry;
using Pagan.Relationships;

namespace Pagan.Tests.TestControllers
{
    [UseAsTableName]
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

        [DbName("Qty")]
        public Column Quantity { get; set; }
        [DbGenerated]
        public Column Cost { get; set; }
        public WithOne<Product> Product { get; set; }
        public WithOne<Order> Order { get; set; }

        internal bool ConfigureWasCalled;
    }
}