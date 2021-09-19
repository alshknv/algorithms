using Xunit;
using _3_strongly_connected;

namespace Tests
{
    public class StronglyConnectedTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("2", StronglyConnected.Solve(new string[]{
                "4 4",
                "1 2",
                "4 1",
                "2 3",
                "3 1"
            }));
        }

        [Fact]
        public void Example2()
        {
            Assert.Equal("5", StronglyConnected.Solve(new string[]{
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

        [Fact]
        public void Example3()
        {
            Assert.Equal("6", StronglyConnected.Solve(new string[]{
                "8 11",
                "1 3",
                "5 3",
                "2 3",
                "7 3",
                "6 3",
                "4 2",
                "4 6",
                "6 4",
                "2 6",
                "7 6",
                "8 4"
            }));
        }
    }
}