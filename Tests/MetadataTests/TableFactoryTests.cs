using NUnit.Framework;
using Pagan.Metadata;

namespace Pagan.Tests.MetadataTests
{
    [TestFixture]
    public class TableFactoryTests
    {
        private TableFactory _testable;

        [SetUp]
        public void Setup()
        {
            _testable = new TableFactory();
        }

        [Test]
        public void ReturnsCachedInstance()
        {
            var t1 = _testable.GetTable<TestTable>();
            var t2 = _testable.GetTable<TestTable>();
            
            Assert.AreSame(t1, t2);
        }

        [Test]
        public void TableInfoEntityType()
        {
            var t = _testable.GetTable<TestTable>();

            Assert.AreEqual(typeof(TestTable), t.EntityType);
        }

        [Test]
        public void TableName()
        {
            var t = _testable.GetTable<TestTable>();

            Assert.AreEqual("TestTable", t.SqlTable.Name);
        }

        [Test]
        public void TableNameFromAttribute()
        {
            var t = _testable.GetTable<TestTable2>();

            Assert.AreEqual("TestTable", t.SqlTable.Name);
        }

        [Test]
        public void SchemaName()
        {
            var t = _testable.GetTable<TestTable>();

            Assert.IsNull(t.SqlTable.Schema);
        }

        [Test]
        public void SchemaNameFromAttribute()
        {
            var t = _testable.GetTable<TestTable2>();

            Assert.AreEqual("dbo", t.SqlTable.Schema);
        }

        [Test]
        public void TableHasFields()
        {
            var t = _testable.GetTable<TestTable>();

            Assert.AreEqual(2, t.Fields.Length);
        }

        [Test]
        public void FieldInfoReferencesTableInfo()
        {
            var t = _testable.GetTable<TestTable>();

            Assert.AreEqual(t, t.Fields[0].Table);
            Assert.AreEqual(t, t.Fields[1].Table);
        }

        [Test]
        public void FieldsReferenceTables()
        {
            var t = _testable.GetTable<TestTable>();

            Assert.AreEqual(t.SqlTable, t.Fields[0].SqlField.Table);
            Assert.AreEqual(t.SqlTable, t.Fields[1].SqlField.Table);
        }

        [Test]
        public void FieldNames()
        {
            var t = _testable.GetTable<TestTable>();

            Assert.AreEqual("Id", t.Fields[0].SqlField.Name);
            Assert.AreEqual("Name", t.Fields[1].SqlField.Name);
        }

        [Test]
        public void FieldNamesWithNameAttribute()
        {
            var t = _testable.GetTable<TestTable2>();

            Assert.AreEqual("Id", t.Fields[0].SqlField.Name);
            Assert.AreEqual("Name", t.Fields[1].SqlField.Name);
        }

        [Test]
        public void PropertyInfo()
        {
            var t = _testable.GetTable<TestTable>();

            Assert.AreEqual("Id", t.Fields[0].PropertyInfo.Name);
            Assert.AreEqual("Name", t.Fields[1].PropertyInfo.Name);
        }
    }
}
