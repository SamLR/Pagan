using Moq;
using NUnit.Framework;
using Pagan.Queries;
using Pagan.Registry;
using Pagan.Tests.Fakes;
using Pagan.Tests.TestControllers;

namespace Pagan.Tests
{
    /// <summary>
    /// Simple tests here, just ensuring that the various methods of turning a column into a filter expression
    /// (operator overloads & methods on the Column object) produce the expected FilterExpression
    /// </summary>
    [TestFixture]
    public class ColumnTests
    {
        private Column _column;
        private Table<Product> _table;

        [SetUp]
        public void Setup()
        {
            _table = new Table<Product>(new Mock<ITableFactory>().Object, new FakeConventions());
            _column = _table.Controller.Id;
        }


        [Test]
        public void ColumnsAreEqualIfTheirTablesAndDbNamesAndControllerNamesAreEqual()
        {
            var c2 = new Column(_table, "Id") {DbName = "Id"};
            Assert.AreEqual(_column, c2);
        }

        [Test]
        public void ColumnsAreNotEqualIfDbNamesOrControllerNamesDiffer()
        {
            var c2 = new Column(_table, "Id") { DbName = "Diff" };
            Assert.AreNotEqual(_column, c2);
        }

        [Test]
        public void EqualToSimpleValue()
        {
            const int test = 10;
            var ex = _column == test;
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.Equal, ex.Operator);
            Assert.AreEqual(test, ex.Value);
        }

        [Test]
        public void NotEqualToSimpleValue()
        {
            const int test = 10;
            var ex = _column != test;
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.NotEqual, ex.Operator);
            Assert.AreEqual(test, ex.Value);
        }

        [Test]
        public void EqualToArray()
        {
            var test = new[] {10, 20, 30};
            var ex = _column == test;
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.In, ex.Operator);
            Assert.AreEqual(test, ex.Value);
        }

        [Test]
        public void NotEqualToArray()
        {
            var test = new[] { 10, 20, 30 };
            var ex = _column != test;
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.NotIn, ex.Operator);
            Assert.AreEqual(test, ex.Value);
        }

        [Test]
        public void IsNull()
        {
            var ex = _column == null;
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.IsNull, ex.Operator);
            Assert.IsNull(ex.Value);
        }

        [Test]
        public void IsNotNull()
        {
            var ex = _column != null;
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.IsNotNull, ex.Operator);
            Assert.IsNull(ex.Value);
        }

        [Test]
        public void IsTrue()
        {
            var ex = (FilterExpression) _column;
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.Equal, ex.Operator);
            Assert.IsTrue((bool)ex.Value);
        }

        [Test]
        public void IsFalse()
        {
            var ex = !_column;
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.Equal, ex.Operator);
            Assert.IsFalse((bool)ex.Value);
        }

        [Test]
        public void GreaterThanSimpleValue()
        {
            const int test = 10;
            var ex = _column > test;
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.GreaterThan, ex.Operator);
            Assert.AreEqual(test, ex.Value);
        }

        [Test]
        public void LessThanSimpleValue()
        {
            const int test = 10;
            var ex = _column < test;
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.LessThan, ex.Operator);
            Assert.AreEqual(test, ex.Value);
        }

        [Test]
        public void GreaterThanOrEqualSimpleValue()
        {
            const int test = 10;
            var ex = _column >= test;
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.GreaterOrEqual, ex.Operator);
            Assert.AreEqual(test, ex.Value);
        }

        [Test]
        public void LessThanOrEqualSimpleValue()
        {
            const int test = 10;
            var ex = _column <= test;
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.LessOrEqual, ex.Operator);
            Assert.AreEqual(test, ex.Value);
        }


        [Test]
        public void StartsWith()
        {
            const string test = "Pagan";
            var ex = _column.StartsWith(test);
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.StartsWith, ex.Operator);
            Assert.AreEqual(test, ex.Value);
        }

        [Test]
        public void DoesNotStartWith()
        {
            const string test = "Pagan";
            var ex = !_column.StartsWith(test);
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.NotStartsWith, ex.Operator);
            Assert.AreEqual(test, ex.Value);
        }

        [Test]
        public void EndsWith()
        {
            const string test = "Pagan";
            var ex = _column.EndsWith(test);
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.EndsWith, ex.Operator);
            Assert.AreEqual(test, ex.Value);
        }

        [Test]
        public void DoesNotEndWith()
        {
            const string test = "Pagan";
            var ex = !_column.EndsWith(test);
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.NotEndsWith, ex.Operator);
            Assert.AreEqual(test, ex.Value);
        }


        [Test]
        public void Contains()
        {
            const string test = "Pagan";
            var ex = _column.Contains(test);
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.Contains, ex.Operator);
            Assert.AreEqual(test, ex.Value);
        }

        [Test]
        public void DoesNotContain()
        {
            const string test = "Pagan";
            var ex = !_column.Contains(test);
            Assert.IsNotNull(ex);
            Assert.AreSame(_column, ex.LeftColumn);
            Assert.AreEqual(Operators.NotContains, ex.Operator);
            Assert.AreEqual(test, ex.Value);
        }

    }
}