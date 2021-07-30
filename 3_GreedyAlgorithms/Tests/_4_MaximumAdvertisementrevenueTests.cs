using System;
using System.Collections.Generic;
using Xunit;
using _4_maximum_advertisement_revenue;

namespace Tests
{
    public class MaximumAdvertisementRevenueTests
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("897", MaximumAdvertisementRevenue.Solve(new List<string>() {
                "1",
                "23",
                "39"
            }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("23", MaximumAdvertisementRevenue.Solve(new List<string>() {
                "3",
                "1 3 -5",
                "-2 4 1"
            }));
        }
    }
}
