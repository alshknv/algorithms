using Xunit;
using _2_toposort;

namespace Tests
{
    public class ToposortTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("4 3 1 2", Toposort.Solve(new string[]{
                "4 3",
                "1 2",
                "4 1",
                "3 1"
            }));
        }

        [Fact]
        public void Example2()
        {
            Assert.Equal("4 3 2 1", Toposort.Solve(new string[]{
                "4 1",
                "3 1"
            }));
        }

        [Fact]
        public void Example3()
        {
            Assert.Equal("5 4 3 2 1", Toposort.Solve(new string[]{
                "5 7",
                "2 1",
                "3 2",
                "3 1",
                "4 3",
                "4 1",
                "5 2",
                "5 3"
            }));
        }
    }
}