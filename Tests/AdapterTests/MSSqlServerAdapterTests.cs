using NUnit.Framework;
using Pagan.Adapters;
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
            var tbl = new SqlTable {Name = "Customers", Schema = "Sales"};
            var actual = _testable.Table(tbl);

            Assert.AreEqual("[Sales].[Customers]", actual);
        }

        [Test]
        public void TableFormatWithoutSchema()
        {
            var tbl = new SqlTable { Name = "Customers"};
            var actual = _testable.Table(tbl);

            Assert.AreEqual("[Customers]", actual);
        }

        [Test]
        public void FieldFormatWithSchema()
        {
            var tbl = new SqlTable { Name = "Customers", Schema = "Sales" };
            var fld = new SqlField {Name = "Id", Table = tbl};
            var actual = _testable.Field(fld);

            Assert.AreEqual("[Sales].[Customers].[Id]", actual);
        }

        [Test]
        public void FieldFormatWithoutSchema()
        {
            var tbl = new SqlTable { Name = "Customers" };
            var fld = new SqlField { Name = "Id", Table = tbl };
            var actual = _testable.Field(fld);

            Assert.AreEqual("[Customers].[Id]", actual);
        }
    }
}
