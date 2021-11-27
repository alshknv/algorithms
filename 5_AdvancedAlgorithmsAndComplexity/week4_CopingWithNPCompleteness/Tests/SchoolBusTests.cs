using _3_school_bus;
using Xunit;

namespace Tests
{
    public class SchoolBusTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Collection(SchoolBus.Solve(new string[] {
                "4 6",
                "1 2 20",
                "1 3 42",
                "1 4 35",
                "2 3 30",
                "2 4 34",
                "3 4 12"
            }),
            l1 => Assert.Equal("97", l1),
            l2 => Assert.Equal("1 4 3 2", l2));
        }

        [Fact]
        public void NoPath()
        {
            Assert.Collection(SchoolBus.Solve(new string[] {
                "4 4",
                "1 2 1",
                "2 3 4",
                "3 4 5",
                "4 2 1"
            }),
            l1 => Assert.Equal("-1", l1));
        }
    }
}