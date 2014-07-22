using System.Linq;
using Moq;
using NUnit.Framework;
using Pagan.Queries;
using Pagan.Registry;
using Pagan.Tests.Fakes;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests.Queries
{
    [TestFixture]
    public class TableQueryTests
    {
        private Table<Product> _table;

        [SetUp]
        public void Setup()
        {
            _table = new Table<Product>(new Mock<ITableFactory>().Object, new FakeConventions());
        }

        [Test]
        public void SelectDefaultsToAllColumns()
        {
            var qry = new Query(_table);
            var parts = qry.Participants.Select(c => c.Column).ToArray();
            Assert.AreEqual(3, parts.Length);
            Assert.Contains(_table.Controller.Id, parts);
            Assert.Contains(_table.Controller.Name, parts);
            Assert.Contains(_table.Controller.SupplierId, parts);
        }

        [Test]
        public void OrderByDefaultsToPrimaryKeyAscending()
        {
            var qry = new Query(_table);
            var sort = qry.Sorting.ToArray();
            Assert.AreEqual(1, sort.Length);
            Assert.AreSame(_table.Controller.Id, sort[0].Column);
            Assert.AreEqual(SortDirection.Ascending, sort[0].Direction);
        }
    }
}
