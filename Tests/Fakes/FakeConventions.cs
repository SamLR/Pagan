using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using Pagan.Registry;
using Pagan.Relationships;

namespace Pagan.Tests.Fakes
{
    class FakeConventions: ITableConventions
    {
        public Mock<ITableConventions> Mock { get; private set; }

        public FakeConventions()
        {
            Mock = new Mock<ITableConventions>();

            Mock
                .Setup(x => x.GetSchemaDbName(It.IsAny<Table>())).Returns("dbo");

            Mock
                .Setup(x => x.GetTableDbName(It.IsAny<Table>()))
                .Returns((Table t) => t.Name);

            Mock
                .Setup(x => x.GetColumnDbName(It.IsAny<Column>()))
                .Returns((Column c) => c.Name);

            Mock
                .Setup(x => x.GetPrimaryKey(It.IsAny<Table>()))
                .Returns((Table t) => new[] {t.Columns.First(x => x.Name == "Id")});

            Mock
                .Setup(x => x.GetForeignKey(It.IsAny<Table>(), It.IsAny<Table>()))
                .Returns((Table p, Table d) => new[] {d.Columns.First(x => x.Name == p.ControllerType.Name + "Id")});

        }

        public string GetSchemaDbName(Table table)
        {
            return Mock.Object.GetSchemaDbName(table);
        }

        public string GetTableDbName(Table table)
        {
            return Mock.Object.GetTableDbName(table);
        }

        public string GetColumnDbName(Column column)
        {
            return Mock.Object.GetColumnDbName(column);
        }

        public Column[] GetPrimaryKey(Table table)
        {
            return Mock.Object.GetPrimaryKey(table);
        }

        public Column[] GetForeignKey(Table principalTable, Table dependentTable)
        {
            return Mock.Object.GetForeignKey(principalTable, dependentTable);
        }
    }
}