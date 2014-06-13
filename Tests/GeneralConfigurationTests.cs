using Moq;
using NUnit.Framework;
using Pagan.Registry;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests
{
    [TestFixture]
    public class GeneralConfigurationTests
    {
        private Controller<OrderDetail> _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new Controller<OrderDetail>(new Mock<IControllerFactory>().Object, new Mock<IDbConfiguration>().Object);
        }

        [Test]
        public void SchemaInstanceIsCreated()
        {
            var s1 = _controller.Instance.Dbo;
            var s2 = _controller.Schema;
            Assert.IsNotNull(s1);
            Assert.IsNotNull(s2);
            Assert.AreSame(s1, s2);
            Assert.AreEqual("Dbo", s2.DbName);
        }

        [Test]
        public void TableInstanceIsCreatedAndReferencesSchema()
        {
            var t1 = _controller.Instance.OrderDetails;
            var t2 = _controller.Table;
            Assert.IsNotNull(t1);
            Assert.IsNotNull(t2);
            Assert.AreSame(t1, t2);
            Assert.AreEqual("OrderDetails", t1.DbName);
            Assert.AreSame(_controller.Schema, _controller.Table.Schema);
        }

        [Test]
        public void ColumnInstancesAreCreatedAndReferenceTable()
        {
            Assert.IsNotNull(_controller.Instance.OrderId);
            Assert.IsNotNull(_controller.Instance.ProductId);
            Assert.IsNotNull(_controller.Instance.Quantity);
            Assert.IsNotNull(_controller.Instance.Cost);

            Assert.AreSame(_controller.Table, _controller.Instance.OrderId.Table);
            Assert.AreSame(_controller.Table, _controller.Instance.ProductId.Table);
            Assert.AreSame(_controller.Table, _controller.Instance.Quantity.Table);
            Assert.AreSame(_controller.Table, _controller.Instance.Cost.Table);
        }

        [Test]
        public void ColumnCollectionContainsAllColumns()
        {
            Assert.AreEqual(4, _controller.Columns.Length);
            Assert.Contains(_controller.Instance.OrderId, _controller.Columns);
            Assert.Contains(_controller.Instance.ProductId, _controller.Columns);
            Assert.Contains(_controller.Instance.Quantity, _controller.Columns);
            Assert.Contains(_controller.Instance.Cost, _controller.Columns);
        }

        [Test]
        public void KeyCollectionContainsKeyColumns()
        {
            Assert.AreEqual(2, _controller.KeyColumns.Length);
            Assert.Contains(_controller.Instance.OrderId, _controller.KeyColumns);
            Assert.Contains(_controller.Instance.ProductId, _controller.KeyColumns);
        }

        [Test]
        public void KeyIndexSetCorrectlyOnColumns()
        {
            Assert.IsTrue(_controller.Instance.OrderId.KeyIndex.HasValue);
            Assert.IsTrue(_controller.Instance.ProductId.KeyIndex.HasValue);
            Assert.AreEqual(0, _controller.Instance.OrderId.KeyIndex.Value);
            Assert.AreEqual(1, _controller.Instance.ProductId.KeyIndex.Value);
        }

        [Test]
        public void ParentReferencesAreCreated()
        {
            var product = _controller.Instance.Product;
            Assert.IsNotNull(product);

            var order = _controller.Instance.Order;
            Assert.IsNotNull(order);
        }
    }
}