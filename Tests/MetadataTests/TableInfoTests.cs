using System;
using NUnit.Framework;
using Pagan.Metadata;

namespace Pagan.Tests.MetadataTests
{
    [TestFixture]
    public class TableInfoTests
    {
        private Table _testable;

        [SetUp]
        public void Setup()
        {
            _testable = new TableFactory().GetTable<TestTable>();
        }

        [Test]
        public void GetField()
        {
            var field = _testable.GetField("id");
            
            Assert.AreEqual("Id", field.SqlField.Name);
        }

        [Test]
        public void GetFieldStrictThrowsForMissing()
        {
            Assert.Throws<InvalidOperationException>(() => _testable.GetField("foo"));
        }

        [Test]
        public void GetFieldNotStrictReturnsNullForMissing()
        {
            var field = _testable.GetField("foo", false);

            Assert.IsNull(field);
        }
    }
}
