using System;
using Xunit;
using _4_least_common_multiple;

namespace Tests
{
    public class _4_LCM_Tests
    {
        [Fact]
        public void _1_LCM_Limit()
        {
            var result = LCM.Calc($"{2 * Math.Pow(10, 7)} {2 * Math.Pow(10, 7)}");
            Assert.Equal("20000000", result);
        }

        [Fact]
        public void _1_LCM_1()
        {
            var result = LCM.Calc("6 8");
            Assert.Equal("24", result);
        }

        [Fact]
        public void _1_LCM_2()
        {
            var result = LCM.Calc("761457 614573");
            Assert.Equal("467970912861", result);
        }
    }
}