using Moq;
using NUnit.Framework;
using Pagan.Registry;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests
{
    [TestFixture]
    public class GeneralConfigurationTests
    {
        private Table<OrderDetail> _table;

        [SetUp]
        public void Setup()
        {
            _table = new Table<OrderDetail>(new Mock<ITableFactory>().Object, new Mock<ITableConventions>().Object);
        }

        [Test]
        public void SchemaInstanceIsCreated()
        {
            var s1 = _table.Controller.Dbo;
            var s2 = _table.Schema;
            Assert.IsNotNull(s1);
            Assert.IsNotNull(s2);
            Assert.AreSame(s1, s2);
            Assert.AreEqual("Dbo", s2.DbName);
        }

        [Test]
        public void TableInstanceIsCreatedAndReferencesSchema()
        {
            var table = _table.Controller.OrderDetails;
            Assert.IsNotNull(table);
            Assert.IsNotNull(_table);
            Assert.AreSame(table, _table);
            Assert.AreEqual("OrderDetails", table.Name);
            Assert.AreSame(_table.Schema, table.Schema);
        }

        [Test]
        public void ColumnInstancesAreCreatedAndReferenceTable()
        {
            Assert.IsNotNull(_table.Controller.OrderId);
            Assert.IsNotNull(_table.Controller.ProductId);
            Assert.IsNotNull(_table.Controller.Quantity);
            Assert.IsNotNull(_table.Controller.Cost);

            Assert.AreSame(_table, _table.Controller.OrderId.Table);
            Assert.AreSame(_table, _table.Controller.ProductId.Table);
            Assert.AreSame(_table, _table.Controller.Quantity.Table);
            Assert.AreSame(_table, _table.Controller.Cost.Table);
        }

        [Test]
        public void ColumnCollectionContainsAllColumns()
        {
            Assert.AreEqual(4, _table.Columns.Length);
            Assert.Contains(_table.Controller.OrderId, _table.Columns);
            Assert.Contains(_table.Controller.ProductId, _table.Columns);
            Assert.Contains(_table.Controller.Quantity, _table.Columns);
            Assert.Contains(_table.Controller.Cost, _table.Columns);
        }

        [Test]
        public void KeyCollectionContainsKeyColumns()
        {
            Assert.AreEqual(2, _table.KeyColumns.Length);
            Assert.Contains(_table.Controller.OrderId, _table.KeyColumns);
            Assert.Contains(_table.Controller.ProductId, _table.KeyColumns);
        }

        [Test]
        public void ParentReferencesAreCreated()
        {
            var product = _table.Controller.Product;
            Assert.IsNotNull(product);

            var order = _table.Controller.Order;
            Assert.IsNotNull(order);
        }
    }
}