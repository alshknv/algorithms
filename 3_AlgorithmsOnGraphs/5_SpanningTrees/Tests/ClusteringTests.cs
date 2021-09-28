using _2_clustering;
using Xunit;

namespace Tests
{
    public class ClusteringTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("2.828427125", Clustering.Solve(new string[] {
                "7 6",
                "4 3",
                "5 1",
                "1 7",
                "2 7",
                "5 7",
                "3 3",
                "7 8",
                "2 8",
                "4 4",
                "6 7",
                "2 6",
                "3"
            }));
        }

        [Fact]
        public void Example2()
        {
            Assert.Equal("5.000000000", Clustering.Solve(new string[] {
                "3 1",
                "1 2",
                "4 6",
                "9 8",
                "9 9",
                "8 9",
                "3 11",
                "4 12",
                "4"
            }));
        }
    }
}