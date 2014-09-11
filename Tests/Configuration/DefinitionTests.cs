using System;
using System.Linq;
using NUnit.Framework;
using Pagan.Configuration;
using Pagan.Relationships;
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

        private IDefinition GetUser()
        {
            return _factory.GetDefinition(typeof(Users));
        }

        private IDefinition GetUserEx()
        {
            return _factory.GetDefinition(typeof(UsersEx));
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
        }

        [Test]
        public void TableName()
        {
            var t = GetUser();

            Assert.NotNull(t.Table);
            Assert.AreEqual("Users", t.Table.Name);
        }

        [Test]
        public void SchemaOptional()
        {
            var t = GetUser();

            Assert.IsNull(t.Schema);
        }

        [Test]
        public void HasFields()
        {
            var t = GetUser();

            Assert.AreEqual(4, t.Fields.Count);
        }

        [Test]
        public void HasKeys()
        {
            var t = GetUser();

            Assert.AreEqual(1, t.Keys.Count);
        }

        [Test]
        public void HasRelationships()
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
        public void RelationshipTypes()
        {
            var t = GetUser();

            Assert.AreSame(typeof(Users), t.Relationships[0].DefiningType);
            Assert.AreSame(typeof(Blogs), t.Relationships[0].RelatedType);
        }

        [Test]
        public void RelationshipEndType()
        {
            var t = GetUser();

            Assert.AreEqual(RelationshipEnd.Principal, t.Relationships[0].RelatesTo);
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
        public void ExplicitTable()
        {
            var t = GetUserEx();

            Assert.AreEqual("Users", t.Table.Name);
            Assert.AreSame(t.Table, ((UsersEx) t.Instance).Users);
        }

        [Test]
        public void ExplicitSchema()
        {
            var t = GetUserEx();

            Assert.AreEqual("Dbo", t.Schema.Name);
            Assert.AreSame(t.Schema, ((UsersEx)t.Instance).Dbo);
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
        public void MissingFieldsThrows()
        {
            Assert.Throws<DefinitionError>(
                () =>
                    _factory.GetDefinition(typeof(NoFieldExample))
                );
        }

        [Test]
        public void MissingKeyThrows()
        {
            Assert.Throws<DefinitionError>(
                () =>
                    _factory.GetDefinition(typeof(NoKeyExample))
                );
        }
    }
}
