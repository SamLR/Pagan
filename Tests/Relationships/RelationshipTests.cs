using System;
using System.Linq;
using NUnit.Framework;
using Pagan.Conditions;
using Pagan.Configuration;
using Pagan.Relationships;
using Pagan.SqlObjects;
using Pagan.Tests.SampleDefinitions;

namespace Pagan.Tests.Relationships
{
    [TestFixture]
    public class RelationshipTests
    {
        private IDefinitionFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new DefinitionFactory();
        }

        private IDefinition GetUsers()
        {
            return _factory.GetDefinition(typeof (Users));
        }

        private IDefinition GetBlogs()
        {
            return _factory.GetDefinition(typeof (Blogs));
        }

        private Relationship GetMissingTwinEnd()
        {
            return _factory.GetDefinition(typeof(NoTwinExample)).Relationships[0];
        }

        private Field GetUserField(string name)
        {
            return GetUserEnd().Definition.Fields.First(x => String.Equals(x.MemberName, name, StringComparison.InvariantCultureIgnoreCase));
        }

        private Field GetBlogField(string name)
        {
            return GetBlogEnd().Definition.Fields.First(x => String.Equals(x.MemberName, name, StringComparison.InvariantCultureIgnoreCase));
        }

        [Test]
        public void GetRelationshipTypeFromDeclaringEnd()
        {
            var blogRel = GetBlogEnd();
            var type = blogRel.GetJoin().Type;

            Assert.AreEqual(RelationshipType.HasMany, type);
        }

        [Test]
        public void GetRelationshipTypeFromTwinEnd()
        {
            var userEnd = GetUserEnd();
            var type = userEnd.GetJoin().Type;

            Assert.AreEqual(RelationshipType.HasMany, type);
        }


        [Test]
        public void GetTableRoleFromDeclaringEnd()
        {
            var blogRel = GetBlogEnd();
            var role = blogRel.GetJoin().Role;

            Assert.AreEqual(RelationshipEnd.Principal, role);
        }

        [Test]
        public void GetTableRoleFromTwinEnd()
        {
            var userEnd = GetUserEnd();
            var role = userEnd.GetJoin().Role;

            Assert.AreEqual(RelationshipEnd.Dependent, role);
        }

        [Test]
        public void GetJoinConditionFromDeclaringEnd()
        {
            var join = GetBlogEnd().GetJoin();
            var condition = (LogicalGroup)join.JoinCondition;
            var conditions = condition.Conditions.Cast<FieldJoin>().ToArray();
            var left = GetUserField("Id");
            var right = GetBlogField("UserId");
            
            Assert.AreEqual(1, conditions.Length);
            Assert.AreSame(left, conditions[0].Left);
            Assert.AreSame(right, conditions[0].Right);
        }

        [Test]
        public void GetJoinConditionFromTwinEnd()
        {
            var join = GetUserEnd().GetJoin();
            var condition = (LogicalGroup) join.JoinCondition;
            var conditions = condition.Conditions.Cast<FieldJoin>().ToArray();
            var left = GetUserField("Id");
            var right = GetBlogField("UserId");

            Assert.AreEqual(1, conditions.Length);
            Assert.AreSame(left, conditions[0].Left);
            Assert.AreSame(right, conditions[0].Right);
        }

        [Test]
        public void MissingTwinThrows()
        {
            Assert.Throws<RelationshipError>(() =>
            {
                var missing = GetMissingTwinEnd();
                missing.GetJoin();
            });
        }
    }
}
