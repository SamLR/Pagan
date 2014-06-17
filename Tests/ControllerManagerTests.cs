using Moq;
using NUnit.Framework;
using Pagan.Registry;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests
{
    [TestFixture]
    public class ControllerManagerTests
    {
        private TableFactory _factory;
            
        [SetUp]
        public void Setup()
        {
            _factory = new TableFactory(new Mock<ITableConventions>().Object);
        }

        [Test]
        public void CreatesSingletonControllers()
        {
            var c1 = _factory.GetTable<OrderDetail>();
            var c2 = _factory.GetTable<OrderDetail>();

            Assert.AreSame(c1,c2);
        }
    }
}