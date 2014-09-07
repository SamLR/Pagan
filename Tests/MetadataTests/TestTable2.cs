using System;
using System.Linq.Expressions;
using Pagan.Linq;
using Pagan.Metadata;

namespace Pagan.Tests.MetadataTests
{
    [TableName("TestTable")]
    [SchemaName("dbo")]
    public class TestTable2
    {
        [FieldName("Id")]
        public int TestTable_Id { get; set; }
        public string Name { get; set; }
    }
}