using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Pagan.DbComponents;
using Pagan.Registry;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests
{
    [TestFixture]
    public class ImplicitConfigurationTests
    {
        private Controller<Product> _controller;
        private Mock<IDbConfiguration> _mockConfig;

        [SetUp]
        public void Setup()
        {
            _mockConfig = new Mock<IDbConfiguration>();
            
            _mockConfig
                .Setup(x => x.GetDefaultSchemaName()).Returns("dbo");
            
            _mockConfig
                .Setup(x => x.SetDefaultColumnDbName(It.IsAny<Column>()))
                .Callback((Column c) => c.DbName = c.Name);

            _mockConfig
                .Setup(x => x.SetDefaultPrimaryKey(It.IsAny<Column[]>()))
                .Callback((IEnumerable<Column> c) =>
                {
                    var id = c.First(x => x.Name == "Id");
                    id.Table.SetKeys(id);
                });

            _controller = new Controller<Product>(new Mock<IControllerFactory>().Object, _mockConfig.Object);
        }

        [Test]
        public void UsesDefaultSchema()
        {
            _mockConfig.Verify(x => x.GetDefaultSchemaName(), Times.Once);
        }

        [Test]
        public void UsesDefaultKey()
        {
            _mockConfig.Verify(x => x.SetDefaultPrimaryKey(It.IsAny<Column[]>()), Times.Once);
        }

        [Test]
        public void UsesDefaultColumnNaming()
        {
            _mockConfig.Verify(x => x.SetDefaultColumnDbName(It.IsAny<Column>()), Times.Exactly(3));
        }


    }
}