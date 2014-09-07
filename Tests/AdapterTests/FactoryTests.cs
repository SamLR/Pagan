using System;
using NUnit.Framework;
using Pagan.Adapters;

namespace Pagan.Tests.AdapterTests
{
    [TestFixture]
    public class FactoryTests
    {
        private AdapterFactory _testable;

        [SetUp]
        public void SetUp()
        {
            _testable = new AdapterFactory();
        }

        [Test]
        public void ReturnsAdapterForSql2008()
        {
            var adapter = _testable.GetAdapter(AdapterFactory.Server.MSSql2008);

            Assert.IsInstanceOf<MSSqlServerAdapter>(adapter);
        }

        [Test]
        public void ReturnsAdapterForSql2012()
        {
            var adapter = _testable.GetAdapter(AdapterFactory.Server.MSSql2012);

            Assert.IsInstanceOf<MSSqlServerAdapter>(adapter);
        }

        [Test]
        public void OracleNotSupported()
        {
            Assert.Throws<Exception>(()=> _testable.GetAdapter(AdapterFactory.Server.Oracle));
        }

        [Test]
        public void MySqlNotSupported()
        {
            Assert.Throws<Exception>(() => _testable.GetAdapter(AdapterFactory.Server.MySql));
        }

        [Test]
        public void PostGreSqlNotSupported()
        {
            Assert.Throws<Exception>(() => _testable.GetAdapter(AdapterFactory.Server.PostGreSql));
        }
    }
}
