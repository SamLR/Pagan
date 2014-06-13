using Moq;
using NUnit.Framework;
using Pagan.Registry;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests
{
    [TestFixture]
    public class ExplicitConfigurationTests
    {
        private Table<OrderDetail> _table;
        private Mock<ITableConfiguration> _mockConfig;

        [SetUp]
        public void Setup()
        {
            _mockConfig = new Mock<ITableConfiguration>();
            _table = new Table<OrderDetail>(new Mock<ITableFactory>().Object, _mockConfig.Object);
        }

        [Test]
        public void DoesNotUseDefaultSchema()
        {
            _mockConfig.Verify(x => x.GetDefaultSchemaName(), Times.Never);
        }

        [Test]
        public void ConfigureMethodIsCalled()
        {
            Assert.IsTrue(_table.Controller.ConfigureWasCalled);
        }
        
        [Test]
        public void DoesNotUseDefaultKey()
        {
            _mockConfig.Verify(x => x.SetDefaultPrimaryKey(It.IsAny<Table>()), Times.Never);
        }

        [Test]
        public void UsesDefaultColumnNaming()
        {
            _mockConfig.Verify(x => x.SetDefaultColumnDbName(It.IsAny<Column>()), Times.Exactly(3));
        }
    }
}