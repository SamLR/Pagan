using Moq;
using NUnit.Framework;
using Pagan.Registry;
using Pagan.Tests.Fakes;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests
{
    [TestFixture]
    public class ExplicitConfigurationTests
    {
        private Table<OrderDetail> _table;
        private FakeConventions _conventions;

        [SetUp]
        public void Setup()
        {
            _conventions = new FakeConventions();
            _table = new Table<OrderDetail>(new Mock<ITableFactory>().Object, _conventions);
        }

        [Test]
        public void DoesNotUseConventionForSchema()
        {
            _conventions.Mock.Verify(x => x.GetSchemaDbName(It.IsAny<Table>()), Times.Never);
            Assert.IsNotNull(_table.Schema);
            Assert.AreEqual("Dbo", _table.Schema.DbName);
        }

        [Test]
        public void ConfigureMethodIsCalledOnController()
        {
            Assert.IsTrue(_table.Controller.ConfigureWasCalled);
        }
        
        [Test]
        public void DoesNotUseConventionForPrimaryKey()
        {
            _conventions.Mock.Verify(x => x.GetPrimaryKey(It.IsAny<Table>()), Times.Never);
            Assert.IsNotNull(_table.KeyColumns);
            Assert.AreEqual(2, _table.KeyColumns.Length);
            Assert.Contains(_table.Controller.OrderId, _table.KeyColumns);
            Assert.Contains(_table.Controller.ProductId, _table.KeyColumns);
        }

        [Test]
        public void UsesConventionForUnattributedColumns()
        {
            _conventions.Mock.Verify(x => x.GetColumnDbName(It.IsAny<Column>()), Times.Exactly(3));
            Assert.AreEqual("OrderId", _table.Controller.OrderId.DbName);
            Assert.AreEqual("ProductId", _table.Controller.ProductId.DbName);
        }

        [Test]
        public void DoesNotUseConventionForAttributedColumns()
        {
            Assert.AreEqual("Qty", _table.Controller.Quantity.DbName);
            Assert.AreEqual("StandardCost", _table.Controller.Cost.DbName);
        }

        [Test]
        public void DoesNotUseConventionForTableName()
        {
            _conventions.Mock.Verify(x => x.GetTableDbName(It.IsAny<Table>()), Times.Never);
            Assert.AreEqual("OrderDetail", _table.DbName);
        }
    }
}