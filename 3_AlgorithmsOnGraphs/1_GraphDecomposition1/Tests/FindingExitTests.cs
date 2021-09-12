using System;
using Xunit;
using _1_finding_exit_from_maze;

namespace Tests
{
    public class FindingExitTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("1", FindingExitFromMaze.Solve(new string[] {
                "4 4",
                "1 2",
                "3 2",
                "4 3",
                "1 4",
                "1 4"}));
        }

        [Fact]
        public void Example2()
        {
            Assert.Equal("0", FindingExitFromMaze.Solve(new string[] {
                "4 2",
                "1 2",
                "3 2",
                "1 4"}));
        }
    }
}
