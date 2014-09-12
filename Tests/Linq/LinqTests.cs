using NUnit.Framework;
using Pagan.Linq;
using Pagan.Tests.SampleDefinitions;

namespace Pagan.Tests.Linq
{
    [TestFixture]
    public class LinqTests
    {
        private Database _db;

        [SetUp]
        public void Setup()
        {
            _db = new Database();
        }

        [Test]
        public void JoinTest()
        {
            var qry =
                from user in _db.Query<Users>()
                where user.FirstName == "pete" && user.Id > 0
                orderby user.LastName descending, user.FirstName
                from blog in user.Blogs
                where blog.Name == "warner"
                orderby blog.Name
                select new {user, blog.Name};
        }
    }
}
