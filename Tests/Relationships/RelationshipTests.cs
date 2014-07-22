using Moq;
using NUnit.Framework;
using Pagan.Queries;
using Pagan.Relationships;
using Pagan.Tests.Fakes;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests.Relationships
{
    /// <summary>
    /// Calling Query(..) on a relationship property creates an expression mapping primary key fields from the principal
    /// table to their foreign key counterparts on the dependent. The dependent's host Table may configure the 
    /// foreign key columns explicitly, or leave it to convention. Therefore we want to test the explicit scenario,
    /// and the various implicit ones.
    /// </summary>
    [TestFixture]
    public class RelationshipTests
    {
        private FakeConventions _conventions;
        private FakeTableFactory _factory;

        [SetUp]
        public void Setup()
        {
            _conventions = new FakeConventions();
            _factory = new FakeTableFactory(_conventions);
        }

        /// <summary>
        /// Test the relationship from our fake OrderDetail dependent to its Product principal.
        /// Since OrderDetail explicitly configures the foreign key fields, we won't expect the conventions to be called.
        /// </summary>
        [Test]
        public void ExplicitlyConfiguredForeignKeyDoesNotUseConventions()
        {
            var orderDetail = _factory.GetTable<OrderDetail>().Controller;

            var rel = orderDetail.Product.Query().Relationship;
            _conventions.Mock.Verify(x => x.GetForeignKey(It.Is<Table>(t=> t.DbName=="Products"), It.Is<Table>(t=> t.DbName=="OrderDetail")), Times.Never);

            Assert.IsNotNull(rel);
            Assert.IsNotNull(rel.JoinExpression);
            Assert.IsNotNull(rel.JoinExpression.RightColumn);
            Assert.AreEqual("ProductId", rel.JoinExpression.RightColumn.Name);
            Assert.AreSame(rel.Dependent.ForeignKeyColumns[0], rel.JoinExpression.RightColumn);
        }

        /// <summary>
        /// Test the relationship from our fake Supplier principal to its Products dependent.
        /// Since Product leaves foreign key selection to convention, expect the conventions to be called
        /// </summary>
        [Test]
        public void UnconfiguredForeignKeyUsesConventions()
        {
            var supplier = _factory.GetTable<Supplier>().Controller;

            supplier.Products.Query();
            
            _conventions.Mock.Verify(x => x.GetForeignKey(It.Is<Table>(t => t.DbName == "Suppliers"), It.Is<Table>(t => t.DbName == "Products")), Times.Once);

        }

        /// <summary>
        /// Ensure that the join expression of the relationship matches the primary key to the foreign key
        /// </summary>
        [Test]
        public void CreatesCorrectJoinExpression()
        {
            var supplier = _factory.GetTable<Supplier>().Controller;

            var rel = supplier.Products.Query().Relationship;

            Assert.IsNotNull(rel);
            Assert.IsNotNull(rel.JoinExpression);

            Assert.IsNotNull(rel.JoinExpression.LeftColumn);
            Assert.AreEqual("Id", rel.JoinExpression.LeftColumn.Name);
            Assert.AreSame(rel.Principal.PrimaryKeyColumns[0], rel.JoinExpression.LeftColumn);

            Assert.IsNotNull(rel.JoinExpression.RightColumn);
            Assert.AreEqual("SupplierId", rel.JoinExpression.RightColumn.Name);
            Assert.AreSame(rel.Dependent.ForeignKeyColumns[0], rel.JoinExpression.RightColumn);

            Assert.AreEqual(Operators.Equal, rel.JoinExpression.Operator);
        }


        /// <summary>
        /// Ensure that the join expression of the relationship matches the primary key to the foreign key
        /// </summary>
        [Test]
        public void UsesCorrectRole()
        {
            var supplier = _factory.GetTable<Supplier>().Controller;

            var hasMany = supplier.Products.Query().Relationship;

            Assert.IsNotNull(hasMany);
            Assert.AreEqual(Role.Dependent, hasMany.Role);

            var product = _factory.GetTable<Product>().Controller;

            var withOne = product.Supplier.Query().Relationship;

            Assert.IsNotNull(withOne);
            Assert.AreEqual(Role.Principal, withOne.Role);
        }
    }
}