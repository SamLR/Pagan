using Moq;
using NUnit.Framework;
using Pagan.Registry;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests
{
    [TestFixture]
    public class ControllerManagerTests
    {
        private ControllerFactory _factory;
            
        [SetUp]
        public void Setup()
        {
            _factory = new ControllerFactory(new Mock<IDbConfiguration>().Object);
        }

        [Test]
        public void CreatesSingletonControllers()
        {
            var c1 = _factory.GetController<OrderDetail>();
            var c2 = _factory.GetController<OrderDetail>();

            Assert.AreSame(c1,c2);
        }
    }
}