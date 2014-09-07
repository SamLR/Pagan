using NUnit.Framework;
using Pagan.Linq;
using Pagan.Tests.MetadataTests;

namespace Pagan.Tests.LinqTests
{
    [TestFixture]
    public class QueryTests
    {
        [Test]
        public void Join()
        {
            var qry =
                from f in new Query<TestTable>()
                join j in new Query<TestTable2>()
                    on new{ f.Id, f.Name}  equals new{ Id = j.TestTable_Id, j.Name}
                select new {f, j};
        }

    }
}
