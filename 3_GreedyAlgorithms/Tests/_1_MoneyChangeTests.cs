using System;
using Xunit;
using _1_money_change;

namespace Tests
{
    public class MoneyChangeTests
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("2", MoneyChange.Solve("2"));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("6", MoneyChange.Solve("28"));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal("104", MoneyChange.Solve("999"));
        }

    }
}
