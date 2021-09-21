using Xunit;
using _2_bipartite;

namespace Tests
{
    public class BipartiteTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("0", Bipartite.Solve(new string[] {
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
            Assert.Equal("1", Bipartite.Solve(new string[] {
                "5 4",
                "5 2",
                "4 2",
                "3 4",
                "1 4"
            }));
        }
    }
}