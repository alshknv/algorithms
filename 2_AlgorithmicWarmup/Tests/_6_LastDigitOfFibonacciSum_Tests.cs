using System;
using Xunit;
using _6_last_digit_of_the_sum_of_fibonacci_numbers;

namespace Tests
{
    public class _6_LastDigitOfFibonacciSum_Tests
    {
        [Fact]
        public void LastDigitOfSum_Limit()
        {
            var result = LastDigitOfSum.Calc($"{Math.Pow(10, 14)}");
            Assert.Equal("5", result);
        }

        [Fact]
        public void LastDigitOfSum_1()
        {
            var result = LastDigitOfSum.Calc("3");
            Assert.Equal("4", result);
        }

        [Fact]
        public void LastDigitOfSum_2()
        {
            var result = LastDigitOfSum.Calc("100");
            Assert.Equal("5", result);
        }
    }
}