using Moq;
using NUnit.Framework;
using Pagan.Tests.Fakes;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests
{
    /// <summary>
    /// Calling Execute(..) on a relationship property creates an expression mapping primary key fields from the principal
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
            
            orderDetail.Product.Query();
            _conventions.Mock.Verify(x => x.GetForeignKey(It.IsAny<Table>(), It.IsAny<Table>()), Times.Never);
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
            _conventions.Mock.Verify(x => x.GetForeignKey(It.IsAny<Table>(), It.IsAny<Table>()), Times.Once);
        }
    }
}