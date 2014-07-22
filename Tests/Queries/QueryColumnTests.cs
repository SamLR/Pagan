using Moq;
using NUnit.Framework;
using Pagan.Queries;
using Pagan.Registry;
using Pagan.Tests.Fakes;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests.Queries
{
    [TestFixture]
    public class QueryColumnTests
    {
        private Table<Product> _table;

        [SetUp]
        public void Setup()
        {
            _table  = new Table<Product>(new Mock<ITableFactory>().Object, new FakeConventions());
        }

        [Test]
        public void QueryColumnsAreEqualIfTheColumnsAreEqual()
        {
            var qc1 = new QueryColumn(_table.Controller.Id);
            var qc2 = new QueryColumn(_table.Controller.Id);
            Assert.AreEqual(qc1, qc2);
        }
        
    }
}