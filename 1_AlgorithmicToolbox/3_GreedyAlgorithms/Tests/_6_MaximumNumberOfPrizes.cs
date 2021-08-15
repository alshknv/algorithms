using System;
using Xunit;
using _6_maximum_number_of_prizes;

namespace Tests
{
    public class MaximumNumberOfPrizesTests
    {
        [Fact]
        public void Test1()
        {
            Assert.Collection(MaximumNumberOfPrizes.Solve("6"),
            l1 => Assert.Equal("3", l1),
            l2 => Assert.Equal("1 2 3", l2));
        }

        [Fact]
        public void Test2()
        {
            Assert.Collection(MaximumNumberOfPrizes.Solve("8"),
            l1 => Assert.Equal("3", l1),
            l2 => Assert.Equal("1 2 5", l2));
        }

        [Fact]
        public void Test3()
        {
            Assert.Collection(MaximumNumberOfPrizes.Solve("2"),
            l1 => Assert.Equal("1", l1),
            l2 => Assert.Equal("2", l2));
        }

        [Fact]
        public void Test4()
        {
            MaximumNumberOfPrizes.Solve("200000000");
        }
    }
}
