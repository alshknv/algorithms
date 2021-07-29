using System;
using Xunit;
using _8_last_digit_of_the_sum_of_squares_of_fibonacci_numbers;

namespace Tests
{
    public class _8_LastDigitOfSumOfSquares_Tests
    {
        [Fact]
        public void _1_LastDigitOfSumOfSquares_Limit()
        {
            var result = LastDigitOfSumOfSquares.Calc($"{Math.Pow(10, 14)}");
            Assert.Equal("5", result);
        }

        [Fact]
        public void _1_LastDigitOfSumOfSquares_1()
        {
            var result = LastDigitOfSumOfSquares.Calc("7");
            Assert.Equal("3", result);
        }

        [Fact]
        public void _1_LastDigitOfSumOfSquares_2()
        {
            var result = LastDigitOfSumOfSquares.Calc("73");
            Assert.Equal("1", result);
        }

        [Fact]
        public void _1_LastDigitOfSumOfSquares_3()
        {
            var result = LastDigitOfSumOfSquares.Calc("1234567890");
            Assert.Equal("0", result);
        }
    }
}