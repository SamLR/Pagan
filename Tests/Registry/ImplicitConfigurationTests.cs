using Moq;
using NUnit.Framework;
using Pagan.Registry;
using Pagan.Tests.Fakes;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests.Registry
{
    [TestFixture]
    public class ImplicitConfigurationTests
    {
        private Table<Product> _table;
        private FakeConventions _conventions;

        [SetUp]
        public void Setup()
        {
            _conventions = new FakeConventions();

            _table = new Table<Product>(new Mock<ITableFactory>().Object, _conventions);
        }

        [Test]
        public void UsesConventionForSchema()
        {
            _conventions.Mock.Verify(x => x.GetSchemaDbName(It.IsAny<Table>()), Times.Once);
            Assert.IsNotNull(_table.Schema);
            Assert.AreEqual("dbo", _table.Schema.DbName);
        }

        [Test]
        public void UsesConventionForPrimaryKey()
        {
            _conventions.Mock.Verify(x => x.GetPrimaryKey(It.IsAny<Table>()), Times.Once);
            Assert.IsNotNull(_table.KeyColumns);
            Assert.AreEqual(1, _table.KeyColumns.Length);
            Assert.Contains(_table.Controller.Id, _table.KeyColumns);
        }

        [Test]
        public void UsesConventionForColumnNaming()
        {
            _conventions.Mock.Verify(x => x.GetColumnDbName(It.IsAny<Column>()), Times.Exactly(3));
            Assert.AreEqual("Id", _table.Controller.Id.DbName);
            Assert.AreEqual("Name", _table.Controller.Name.DbName);
            Assert.AreEqual("SupplierId", _table.Controller.SupplierId.DbName);
        }

        [Test]
        public void UsesConventionForTableNaming()
        {
            _conventions.Mock.Verify(x => x.GetTableDbName(It.IsAny<Table>()), Times.Once);
            Assert.AreEqual("Products", _table.Controller.Products.DbName);
            Assert.AreEqual("Products", _table.DbName);
        }


    }
}