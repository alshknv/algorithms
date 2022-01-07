using System;
using Xunit;
using _1_circulation;

namespace Tests
{
    public class CirculationTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Collection(Circulation.Solve(new string[] { "3 2", "1 2 0 3", "2 3 0 3" }),
            l1 => Assert.Equal("YES", l1),
            l2 => Assert.Equal("0", l2),
            l3 => Assert.Equal("0", l3));
        }

        [Fact]
        public void Example2()
        {
            Assert.Collection(Circulation.Solve(new string[] { "3 3", "1 2 1 3", "2 3 2 4", "3 1 1 2" }),
            l1 => Assert.Equal("YES", l1),
            l2 => Assert.Equal("2", l2),
            l3 => Assert.Equal("2", l3),
            l4 => Assert.Equal("2", l4));
        }

        [Fact]
        public void Example3()
        {
            Assert.Collection(Circulation.Solve(new string[] { "3 3", "1 2 1 3", "2 3 2 4", "1 3 1 2" }),
            l1 => Assert.Equal("NO", l1));
        }
    }
}
