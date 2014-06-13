using Pagan.DbComponents;
using Pagan.Relationships;

namespace Pagan.Tests.TestControllers
{
    public class Order
    {
        public Table Orders;
        public Column OrderId { get; set; }
        public Column CustomerId { get; set; }
        public Column OrderDate { get; set; }
        public HasMany<OrderDetail> Details { get; set; }
    }
}