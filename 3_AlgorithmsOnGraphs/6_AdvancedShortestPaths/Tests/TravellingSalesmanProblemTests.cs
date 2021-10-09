using Xunit;
using _5_travelling_salesman_problem;

namespace Tests
{
    public class TravellingSalesmanProblemTests
    {
        [Fact]
        public void Example1()
        {
            TravellingSalesmanProblem.Preprocess(4, new string[] {
                "1 2 1",
                "2 3 1",
                "3 4 1",
                "4 1 1",
                "2 1 1"
            });
            Assert.Collection(TravellingSalesmanProblem.ProcessQueries(new string[] {
                "2 1 2",
                "2 1 3",
                "4 1 2 3 4" }),
            l1 => Assert.Equal("2", l1),
            l2 => Assert.Equal("4", l2),
            l3 => Assert.Equal("4", l3));
        }

        [Fact]
        public void Example2()
        {
            TravellingSalesmanProblem.Preprocess(5, new string[] {
                "1 2 1",
                "2 3 1",
                "3 4 1",
                "4 1 1",
                "2 1 1",
                "4 5 1"
            });
            Assert.Collection(TravellingSalesmanProblem.ProcessQueries(new string[] {
                "5 1 2 3 4 5" }),
            l1 => Assert.Equal("-1", l1));
        }
    }
}