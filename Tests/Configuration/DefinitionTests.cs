using System;
using System.Linq;
using NUnit.Framework;
using Pagan.Configuration;
using Pagan.SqlObjects;
using Pagan.Tests.SampleDefinitions;

namespace Pagan.Tests.Configuration
{
    [TestFixture]
    public class DefinitionTests
    {
        private DefinitionFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new DefinitionFactory();
        }

        private Definition<User> GetUser()
        {
            return _factory.GetDefinition<User>();
        }

        private Definition<UserEx> GetUserEx()
        {
            return _factory.GetDefinition<UserEx>();
        }

        private Field GetUserField(string name)
        {
            return GetUser().Fields.First(x => String.Equals(x.MemberName, name, StringComparison.InvariantCultureIgnoreCase));
        }

        private Field GetUserFieldEx(string name)
        {
            return GetUserEx().Fields.First(x => String.Equals(x.MemberName, name, StringComparison.InvariantCultureIgnoreCase));
        }

        [Test]
        public void FactoryReturnsCachedInstance()
        {
            var t1 = GetUser();
            var t2 = GetUser();

            Assert.AreSame(t1, t2);
        }

        [Test]
        public void InstanceCreated()
        {
            var t = GetUser();
            Assert.NotNull(t.Instance);
        }

        [Test]
        public void TableCreated()
        {
            var t = GetUser();

            Assert.NotNull(t.Table);
            Assert.NotNull(t.Instance.Users);

            Assert.AreSame(t.Table, t.Instance.Users);
        }

        [Test]
        public void TableName()
        {
            var t = GetUser();

            Assert.AreEqual("Users", t.Table.Name);
        }

        [Test]
        public void SchemaCreated()
        {
            var t = GetUser();

            Assert.NotNull(t.Schema);
            Assert.NotNull(t.Instance.Dbo);

            Assert.AreSame(t.Schema, t.Instance.Dbo);
        }

        [Test]
        public void SchemaName()
        {
            var t = GetUser();

            Assert.AreEqual("Dbo", t.Schema.Name);
        }

        [Test]
        public void TableHasFields()
        {
            var t = GetUser();

            Assert.AreEqual(4, t.Fields.Count);
        }

        [Test]
        public void TableHasKeys()
        {
            var t = GetUser();

            Assert.AreEqual(1, t.Keys.Count);
        }

        [Test]
        public void TableHasRelationships()
        {
            var t = GetUser();

            Assert.AreEqual(1, t.Relationships.Count);
        }

        [Test]
        public void RelationshipName()
        {
            var t = GetUser();

            Assert.AreEqual("Blogs", t.Relationships[0].Name);
        }

        [Test]
        public void FieldNames()
        {
            var id = GetUserField("Id");
            var first = GetUserField("FirstName");
            var last = GetUserField("LastName");
            var email = GetUserField("Email");

            Assert.AreEqual("Id", id.Name);
            Assert.AreEqual("FirstName", first.Name);
            Assert.AreEqual("LastName", last.Name);
            Assert.AreEqual("Email", email.Name);
        }

        [Test]
        public void ExplicitTableName()
        {
            var t = GetUserEx();

            Assert.AreEqual("UserEx", t.Table.Name);
        }

        [Test]
        public void ExplicitFieldNames()
        {
            var id = GetUserFieldEx("Id");
            var first = GetUserFieldEx("FirstName");
            var last = GetUserFieldEx("LastName");
            var email = GetUserFieldEx("Email");

            Assert.AreEqual("Id", id.Name);
            Assert.AreEqual("First_Name", first.Name);
            Assert.AreEqual("Last_Name", last.Name);
            Assert.AreEqual("Email", email.Name);
        }

        [Test]
        public void MissingTableMemberThrows()
        {
            Assert.Throws<DefinitionError>(
                () =>
                    _factory.GetDefinition<NoTableExample>()
                );
        }

        [Test]
        public void MissingKeyMemberThrows()
        {
            Assert.Throws<DefinitionError>(
                () =>
                    _factory.GetDefinition<NoKeyExample>()
                );
        }
    }
}
