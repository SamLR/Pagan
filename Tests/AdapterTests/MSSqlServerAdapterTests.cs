using NUnit.Framework;
using Pagan.Adapters;
using Pagan.Configuration;
using Pagan.SqlObjects;

namespace Pagan.Tests.AdapterTests
{
    [TestFixture]
    public class MSSqlServerAdapterTests
    {
        private ISqlAdapter _testable;

        [SetUp]
        public void Setup()
        {
            _testable = new AdapterFactory().GetAdapter(AdapterFactory.Server.MSSql2012);
        }

        [Test]
        public void TableFormatWithSchema()
        {
            var tbl = new Table("Customers", null) {Schema = new Schema("Sales", null)};
            var actual = _testable.Table(tbl);

            Assert.AreEqual("[Sales].[Customers]", actual);
        }

        [Test]
        public void TableFormatWithoutSchema()
        {
            var tbl = new Table("Customers", null);
            var actual = _testable.Table(tbl);

            Assert.AreEqual("[Customers]", actual);
        }

        [Test]
        public void FieldFormatWithSchema()
        {
            var tbl = new Table("Customers", null) { Schema = new Schema("Sales", null) };
            var fld = new Field("Id", null) {Table = tbl};
            var actual = _testable.Field(fld);

            Assert.AreEqual("[Sales].[Customers].[Id]", actual);
        }

        [Test]
        public void FieldFormatWithoutSchema()
        {
            var tbl = new Table("Customers", null);
            var fld = new Field("Id", null) { Table = tbl }; 
            var actual = _testable.Field(fld);

            Assert.AreEqual("[Customers].[Id]", actual);
        }
    }
}
