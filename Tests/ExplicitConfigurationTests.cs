using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Pagan.DbComponents;
using Pagan.Registry;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests
{
    [TestFixture]
    public class ExplicitConfigurationTests
    {
        private Controller<OrderDetail> _controller;
        private Mock<IDbConfiguration> _mockConfig;

        [SetUp]
        public void Setup()
        {
            _mockConfig = new Mock<IDbConfiguration>();
            _controller = new Controller<OrderDetail>(new Mock<IControllerFactory>().Object, _mockConfig.Object);
        }

        [Test]
        public void DoesNotUseDefaultSchema()
        {
            _mockConfig.Verify(x => x.GetDefaultSchemaName(), Times.Never);
        }

        [Test]
        public void ConfigureMethodIsCalled()
        {
            Assert.IsTrue(_controller.Instance.ConfigureWasCalled);
        }
        
        [Test]
        public void DoesNotUseDefaultKey()
        {
            _mockConfig.Verify(x => x.SetDefaultPrimaryKey(It.IsAny<Column[]>()), Times.Never);
        }

        [Test]
        public void UsesDefaultColumnNaming()
        {
            _mockConfig.Verify(x => x.SetDefaultColumnDbName(It.IsAny<Column>()), Times.Exactly(3));
        }
    }
}