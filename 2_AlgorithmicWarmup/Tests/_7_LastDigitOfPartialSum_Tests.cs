using System;
using Xunit;
using _7_last_digit_of_the_sum_of_fibonacci_numbers_again;

namespace Tests
{
    public class _7_LastDigitOfPartialSum_Tests
    {
        [Fact]
        public void _1_LastDigitOfPartSum_Limit()
        {
            var result = LastDigitOfPartSum.Calc($"{Math.Pow(10, 14)} {Math.Pow(10, 14)}");
            Assert.Equal("5", result);
        }

        [Fact]
        public void _1_LastDigitOfPartSum_1()
        {
            var result = LastDigitOfPartSum.Calc("3 7");
            Assert.Equal("1", result);
        }

        [Fact]
        public void _1_LastDigitOfPartSum_2()
        {
            var result = LastDigitOfPartSum.Calc("10 10");
            Assert.Equal("5", result);
        }

        [Fact]
        public void _1_LastDigitOfPartSum_3()
        {
            var result = LastDigitOfPartSum.Calc("10 200");
            Assert.Equal("2", result);
        }
    }
}