using Xunit;
using _2_adding_exits_to_maze;

namespace Tests
{
    public class AddingExitsTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("1", AddingExitsToMaze.Solve(new string[] {
                "4 4",
                "1 2",
                "3 2",
                "4 3",
                "1 4"}));
        }

        [Fact]
        public void Example2()
        {
            Assert.Equal("2", AddingExitsToMaze.Solve(new string[] {
                "4 2",
                "1 2",
                "3 2"}));
        }

        [Fact]
        public void Example3()
        {
            Assert.Equal("1", AddingExitsToMaze.Solve(new string[] {
                "1 0"}));
        }
    }
}