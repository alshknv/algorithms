using System;
using System.Collections.Generic;
using Xunit;
using _3_car_fueling;

namespace Tests
{
    public class CarFuelingTests
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("2", CarFueling.Solve(new List<string>() {
                "950",
                "400",
                "4",
                "200 375 550 750"
            }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("-1", CarFueling.Solve(new List<string>() {
                "10",
                "3",
                "4",
                "1 2 5 9"
            }));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal("0", CarFueling.Solve(new List<string>() {
                "200",
                "250",
                "2",
                "100 150"
            }));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal("2", CarFueling.Solve(new List<string>() {
                "500",
                "200",
                "4",
                "100 200 300 400"
            }));
        }
    }
}
