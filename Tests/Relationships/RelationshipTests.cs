using NUnit.Framework;
using Pagan.Conditions;
using Pagan.Configuration;
using Pagan.Relationships;
using Pagan.Tests.SampleDefinitions;

namespace Pagan.Tests.Relationships
{
    [TestFixture]
    public class RelationshipTests
    {
        private IRelationshipResolver _resolver;
        private IDefinitionFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new DefinitionFactory();
            _resolver = new RelationshipResolver(_factory);
        }

        [Test]
        public void GetJoinFromDeclaringEnd()
        {
            var join = _resolver.GetJoin(typeof (Blogs), typeof (Users));

            Assert.NotNull(join);
            Assert.AreEqual("Users", join.Table.Name);
            Assert.AreEqual(RelationshipEnd.Principal, join.End);
            Assert.AreEqual(Multiplicity.One, join.Multiplicity);
            
            var condition = (FieldJoin) join.JoinCondition;
            Assert.AreEqual("Users", condition.Left.Table.Name);
            Assert.AreEqual("Blogs", condition.Right.Table.Name);

            Assert.AreEqual("Id", condition.Left.Name);
            Assert.AreEqual("UserId", condition.Right.Name);
        }

        [Test]
        public void GetJoinFromRelatedEnd()
        {
            var join = _resolver.GetJoin(typeof(Users), typeof(Blogs));

            Assert.NotNull(join);
            Assert.AreEqual("Blogs", join.Table.Name);
            Assert.AreEqual(RelationshipEnd.Dependent, join.End);
            Assert.AreEqual(Multiplicity.Many, join.Multiplicity);

            var condition = (FieldJoin)join.JoinCondition;
            Assert.AreEqual("Users", condition.Left.Table.Name);
            Assert.AreEqual("Blogs", condition.Right.Table.Name);

            Assert.AreEqual("Id", condition.Left.Name);
            Assert.AreEqual("UserId", condition.Right.Name);
        }

        [Test]
        public void UndefinedMappingThrows()
        {
            Assert.Throws<RelationshipError>(() => _resolver.GetJoin(typeof (BadRelationsExample), typeof (Users)));
        }

        [Test]
        public void UndefinedRelationshipThrows()
        {
            Assert.Throws<RelationshipError>(() => _resolver.GetJoin(typeof(BadRelationsExample), typeof(Blogs)));
        }

    }
}
