using System;
using Xunit;
using _2_last_digit_of_fibonacci_number;

namespace Tests
{
    public class _2_FibonacciLastDigit_Tests
    {
        [Fact]
        public void FibonacciLastDigit_Limit()
        {
            var result = FibonacciLastDigit.Calc(Math.Pow(10, 7).ToString("0"));
            Assert.Equal("5", result);
        }

        [Fact]
        public void FibonacciLastDigit_1()
        {
            var result = FibonacciLastDigit.Calc("3");
            Assert.Equal("2", result);
        }

        [Fact]
        public void FibonacciLastDigit_2()
        {
            var result = FibonacciLastDigit.Calc("331");
            Assert.Equal("9", result);
        }

        [Fact]
        public void FibonacciLastDigit_3()
        {
            var result = FibonacciLastDigit.Calc("240");
            Assert.Equal("0", result);
        }
    }
}
