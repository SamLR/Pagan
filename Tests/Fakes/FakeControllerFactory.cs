using Moq;
using Pagan.Registry;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests.Fakes
{
    class FakeControllerFactory: IControllerFactory
    {
        Controller<OrderDetail> _orderDetail;
        Controller<Product> _product;
        Controller<Supplier> _supplier;

        public Mock<IControllerFactory> Mock { get; private set; }

        public FakeControllerFactory(IDbConfiguration config)
        {
            Mock = new Mock<IControllerFactory>();

            Mock
                .Setup(x =>
                    x.GetController<Product>())
                .Returns(() =>
                    _product ?? (_product = new Controller<Product>(Mock.Object, config)));

            Mock
                .Setup(x =>
                    x.GetController<OrderDetail>())
                .Returns(() =>
                    _orderDetail ?? (_orderDetail = new Controller<OrderDetail>(Mock.Object, config)));

            Mock
                .Setup(x =>
                    x.GetController<Supplier>())
                .Returns(() =>
                    _supplier ?? (_supplier = new Controller<Supplier>(Mock.Object, config)));
        }

        public Controller<T> GetController<T>()
        {
            return Mock.Object.GetController<T>();
        }
    }
}
