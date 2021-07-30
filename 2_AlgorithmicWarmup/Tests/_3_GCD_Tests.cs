using System;
using Xunit;
using _3_greatest_common_divisor;

namespace Tests
{
    public class _3_GCD_Tests
    {
        [Fact]
        public void GCD_Limit()
        {
            var result = GCD.Calc($"{2 * Math.Pow(10, 9)} {2 * Math.Pow(10, 9)}");
            Assert.Equal("2000000000", result);
        }

        [Fact]
        public void GCD_1()
        {
            var result = GCD.Calc("18 35");
            Assert.Equal("1", result);
        }

        [Fact]
        public void GCD_2()
        {
            var result = GCD.Calc("28851538 1183019");
            Assert.Equal("17657", result);
        }
    }
}