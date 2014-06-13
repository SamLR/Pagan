using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Pagan.Registry;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests
{
    [TestFixture]
    public class ImplicitConfigurationTests
    {
        private Table<Product> _table;
        private Mock<ITableConfiguration> _mockConfig;

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
                    t.SetKey(id);
                });

            _table = new Table<Product>(new Mock<ITableFactory>().Object, _mockConfig.Object);
        }

        [Test]
        public void UsesDefaultSchema()
        {
            _mockConfig.Verify(x => x.GetDefaultSchemaName(), Times.Once);
        }

        [Test]
        public void UsesDefaultKey()
        {
            _mockConfig.Verify(x => x.SetDefaultPrimaryKey(It.IsAny<Table>()), Times.Once);
        }

        [Test]
        public void UsesDefaultColumnNaming()
        {
            _mockConfig.Verify(x => x.SetDefaultColumnDbName(It.IsAny<Column>()), Times.Exactly(3));
        }


    }
}