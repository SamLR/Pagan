using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Pagan.Registry;
using Pagan.Relationships;
using Pagan.Tests.Fakes;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests
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
        private Mock<ITableConfiguration> _mockConfig;
        private ITableFactory _factory;

        [SetUp]
        public void Setup()
        {

            _mockConfig = new Mock<ITableConfiguration>();

            _mockConfig
                .Setup(x => x.GetDefaultSchemaName()).Returns("dbo");

            _mockConfig
                .Setup(x => x.SetDefaultColumnDbName(It.IsAny<Column>()))
                .Callback((Column c) => c.DbName = c.Name);

            _mockConfig
                .Setup(x => x.SetDefaultPrimaryKey(It.IsAny<Table>()))
                .Callback((Table t) =>
                {
                    var id = t.Columns.First(x => x.Name == "Id");
                    id.Table.SetKey(id);
                });

            _mockConfig
                .Setup(x => x.SetDefaultForeignKey(It.IsAny<IDependent>(), It.IsAny<Column[]>()))
                .Callback((IDependent d, IEnumerable<Column> c) =>
                {
                    var supplierId = c.First(x => x.Name == "SupplierId");
                    d.SetForeignKey(supplierId);
                });


            _factory = new FakeTableFactory(_mockConfig.Object);
        }

        /// <summary>
        /// Test the relationship from our fake OrderDetail dependent to its Product principal.
        /// Since OrderDetail explicitly configures the foreign key fields, we won't expect the configuration to be called.
        /// </summary>
        [Test]
        public void ExplicitlyConfiguredForeignKeyDoesNotUseConventions()
        {
            var orderDetail = _factory.GetTable<OrderDetail>().Controller;
            
            orderDetail.Product.Query();
            _mockConfig.Verify(x => x.SetDefaultForeignKey(It.IsAny<IDependent>(), It.IsAny<Column[]>()), Times.Never);
        }

        /// <summary>
        /// Test the relationship from our fake Supplier principal to its Products dependent.
        /// Since Product leaves foreign key selection to convention, expect the configuration to be called
        /// </summary>
        [Test]
        public void UnconfiguredForeignKeyUsesConventions()
        {
            var supplier = _factory.GetTable<Supplier>().Controller;
            
            supplier.Products.Query();
            _mockConfig.Verify(x => x.SetDefaultForeignKey(It.IsAny<IDependent>(), It.IsAny<Column[]>()), Times.Once);
        }
    }
}