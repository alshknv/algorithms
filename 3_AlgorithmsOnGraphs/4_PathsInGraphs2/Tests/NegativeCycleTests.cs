using _2_detecting_anomalies;
using Xunit;

namespace Tests
{
    public class NegativeCycleTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("0", NegativeCycle.Solve(new string[] {
                "4 4",
                "1 2 1",
                "4 1 2",
                "2 3 2",
                "1 3 5"
            }));
        }

        [Fact]
        public void Example2()
        {
            Assert.Equal("1", NegativeCycle.Solve(new string[] {
                "4 4",
                "1 2 -5",
                "4 1 2",
                "2 3 2",
                "3 1 1"
            }));
        }
    }
}