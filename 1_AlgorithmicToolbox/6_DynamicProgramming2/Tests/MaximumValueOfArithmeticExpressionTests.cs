using System;
using Xunit;
using _3_maximum_value_of_an_arithmetic_expression;

namespace Tests
{
    public class MaximumValueOfArithmeticExpressionTests
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("6", MaximumValueOfArithmeticExpression.Solve("1+5"));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("200", MaximumValueOfArithmeticExpression.Solve("5-8+7*4-8+9"));
        }
    }
}
