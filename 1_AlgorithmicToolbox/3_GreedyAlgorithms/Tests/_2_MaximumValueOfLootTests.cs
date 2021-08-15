using System;
using System.Collections.Generic;
using Xunit;
using _2_maximum_value_of_the_loot;

namespace Tests
{
    public class MaximumValueOfLootTests
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("180.0000", MaximumValueOfTheLoot.Solve(new List<string>() {
                "3 50",
                "60 20",
                "100 50",
                "120 30"
            }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("166.6667", MaximumValueOfTheLoot.Solve(new List<string>() {
                "1 10",
                "500 30"
            }));
        }
    }
}
