using NUnit.Framework;
using Pagan.Conditions;
using Pagan.SqlObjects;

namespace Pagan.Tests.Conditions
{
    [TestFixture]
    public class LogicalGroupTests
    {
        private static FieldComparison GetCondition()
        {
            return new FieldComparison(new Field("Member"), null, ComparisonOperator.Equal);
        }

        private static void Check(LogicalGroup result, int expectedCount, LogicalOperator expectedOperator)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedOperator, result.Operator);
            Assert.AreEqual(expectedCount, result.Conditions.Count);
        }

        [Test]
        public void Condition_AND_Condition()
        {
            var result = (GetCondition() && GetCondition()) as LogicalGroup;

            Check(result, 2, LogicalOperator.And);
        }

        [Test]
        public void Condition_OR_Condition()
        {
            var result = (GetCondition() || GetCondition()) as LogicalGroup;

            Check(result, 2, LogicalOperator.Or);
        }

        [Test]
        public void AndGroup_AND_Condition()
        {
            var result = (GetCondition() && GetCondition() && GetCondition()) as LogicalGroup;

            Check(result, 3, LogicalOperator.And);
        }

        [Test]
        public void AndGroup_OR_Condition()
        {
            var result = ((GetCondition() && GetCondition()) || GetCondition()) as LogicalGroup;

            Check(result, 2, LogicalOperator.Or);

            var inner = result.Conditions[0] as LogicalGroup;
            Check(inner, 2, LogicalOperator.And);
        }

        [Test]
        public void OrGroup_OR_Condition()
        {
            var result = (GetCondition() || GetCondition() || GetCondition()) as LogicalGroup;

            Check(result, 3, LogicalOperator.Or);
        }

        [Test]
        public void OrGroup_AND_Condition()
        {
            var result = ((GetCondition() || GetCondition()) && GetCondition()) as LogicalGroup;

            Check(result, 2, LogicalOperator.And);

            var inner = result.Conditions[0] as LogicalGroup;
            Check(inner, 2, LogicalOperator.Or);
        }

        [Test]
        public void AndGroup_AND_AndGroup()
        {
            var result = ((GetCondition() && GetCondition()) && (GetCondition() && GetCondition())) as LogicalGroup;

            Check(result, 4, LogicalOperator.And);
        }

        [Test]
        public void AndGroup_AND_OrGroup()
        {
            var result = ((GetCondition() && GetCondition()) && (GetCondition() || GetCondition())) as LogicalGroup;

            Check(result, 3, LogicalOperator.And);

            var inner = result.Conditions[2] as LogicalGroup;

            Check(inner, 2, LogicalOperator.Or);
        }

        [Test]
        public void AndGroup_OR_AndGroup()
        {
            var result = ((GetCondition() && GetCondition()) || (GetCondition() && GetCondition())) as LogicalGroup;

            Check(result, 2, LogicalOperator.Or);

            var left = result.Conditions[0] as LogicalGroup;
            var right = result.Conditions[1] as LogicalGroup;

            Check(left, 2, LogicalOperator.And);
            Check(right, 2, LogicalOperator.And);
        }

        [Test]
        public void AndGroup_OR_OrGroup()
        {
            var result = ((GetCondition() && GetCondition()) || (GetCondition() || GetCondition())) as LogicalGroup;

            Check(result, 3, LogicalOperator.Or);

            var inner = result.Conditions[2] as LogicalGroup;

            Check(inner, 2, LogicalOperator.And);
        }

        [Test]
        public void OrGroup_AND_OrGroup()
        {
            var result = ((GetCondition() || GetCondition()) && (GetCondition() || GetCondition())) as LogicalGroup;

            Check(result, 2, LogicalOperator.And);

            var left = result.Conditions[0] as LogicalGroup;
            var right = result.Conditions[1] as LogicalGroup;

            Check(left, 2, LogicalOperator.Or);
            Check(right, 2, LogicalOperator.Or);
        }

        [Test]
        public void OrGroup_OR_OrGroup()
        {
            var result = ((GetCondition() || GetCondition()) || (GetCondition() || GetCondition())) as LogicalGroup;

            Check(result, 4, LogicalOperator.Or);
        }

    }
}
