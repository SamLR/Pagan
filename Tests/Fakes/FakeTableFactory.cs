using Moq;
using Pagan.Registry;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests.Fakes
{
    class FakeTableFactory: ITableFactory
    {
        Table<OrderDetail> _orderDetail;
        Table<Product> _product;
        Table<Supplier> _supplier;

        public Mock<ITableFactory> Mock { get; private set; }

        public FakeTableFactory(ITableConventions config)
        {
            Mock = new Mock<ITableFactory>();

            Mock
                .Setup(x =>
                    x.GetTable<Product>())
                .Returns(() =>
                    _product ?? (_product = new Table<Product>(Mock.Object, config)));

            Mock
                .Setup(x =>
                    x.GetTable<OrderDetail>())
                .Returns(() =>
                    _orderDetail ?? (_orderDetail = new Table<OrderDetail>(Mock.Object, config)));

            Mock
                .Setup(x =>
                    x.GetTable<Supplier>())
                .Returns(() =>
                    _supplier ?? (_supplier = new Table<Supplier>(Mock.Object, config)));
        }

        public Table<T> GetTable<T>()
        {
            return Mock.Object.GetTable<T>();
        }
    }
}
