using System;
using Xunit;
using _1_fibonacci_number;

namespace Tests
{
    public class _1_Fibonacci_Tests
    {
        [Fact]
        public void _1_Fibonacci_Limit()
        {
            var result = Fibonacci.Calc("45");
            Assert.Equal("1134903170", result);
        }

        [Fact]
        public void _1_Fibonacci_1()
        {
            var result = Fibonacci.Calc("10");
            Assert.Equal("55", result);
        }
    }
}
