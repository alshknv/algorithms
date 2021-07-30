using System;
using Xunit;
using _5_fibonacci_number_again;

namespace Tests
{
    public class _5_FibonacciModulo_Tests
    {
        [Fact]
        public void FibonacciModulo_Limit()
        {
            var result = FibonacciModulo.Calc($"{Math.Pow(10, 14)} 1000");
            Assert.Equal("875", result);
        }

        [Fact]
        public void FibonacciModulo_1()
        {
            var result = FibonacciModulo.Calc("239 1000");
            Assert.Equal("161", result);
        }

        [Fact]
        public void FibonacciModulo_2()
        {
            var result = FibonacciModulo.Calc("2816213588 239");
            Assert.Equal("151", result);
        }
    }
}