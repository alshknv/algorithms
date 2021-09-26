using _3_exchanging_money;
using Xunit;

namespace Tests
{
    public class MoneyExchangeTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Collection(MoneyExchange.Solve(new string[] {
                "6 7",
                "1 2 10",
                "2 3 5",
                "1 3 100",
                "3 5 7",
                "5 4 10",
                "4 3 -18",
                "6 1 -1",
                "1"
            }),
            l1 => Assert.Equal("0", l1),
            l2 => Assert.Equal("10", l2),
            l3 => Assert.Equal("-", l3),
            l4 => Assert.Equal("-", l4),
            l5 => Assert.Equal("-", l5),
            l6 => Assert.Equal("*", l6));
        }

        [Fact]
        public void Example2()
        {
            Assert.Collection(MoneyExchange.Solve(new string[] {
                "5 4",
                "1 2 1",
                "4 1 2",
                "2 3 2",
                "3 1 -5",
                "4"
            }),
            l1 => Assert.Equal("-", l1),
            l2 => Assert.Equal("-", l2),
            l3 => Assert.Equal("-", l3),
            l4 => Assert.Equal("0", l4),
            l5 => Assert.Equal("*", l5));
        }

        [Fact]
        public void Example3()
        {
            Assert.Collection(MoneyExchange.Solve(new string[] {
                "6 8",
                "2 3 -1",
                "3 2 -1",
                "1 4 1",
                "1 5 1",
                "1 6 1",
                "2 4 1",
                "2 5 1",
                "2 6 1",
                "1"
            }),
            l1 => Assert.Equal("0", l1),
            l2 => Assert.Equal("*", l2),
            l3 => Assert.Equal("*", l3),
            l4 => Assert.Equal("1", l4),
            l5 => Assert.Equal("1", l5),
            l6 => Assert.Equal("1", l6));
        }

        [Fact]
        public void Example4()
        {
            Assert.Collection(MoneyExchange.Solve(new string[] {
                "6 7",
                "1 2 10",
                "2 3 5",
                "3 5 7",
                "5 4 10",
                "4 3 -18",
                "3 1 100",
                "6 1 -1",
                "6"
            }),
            l1 => Assert.Equal("-", l1),
            l2 => Assert.Equal("-", l2),
            l3 => Assert.Equal("-", l3),
            l4 => Assert.Equal("-", l4),
            l5 => Assert.Equal("-", l5),
            l6 => Assert.Equal("0", l6));
        }
    }
}