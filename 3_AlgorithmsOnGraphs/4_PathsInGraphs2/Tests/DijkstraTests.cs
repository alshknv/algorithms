using System;
using Xunit;
using _1_minimum_flight_cost;

namespace Tests
{
    public class DijkstraTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("3", Dijkstra.Solve(new string[] {
                "4 4",
                "1 2 1",
                "4 1 2",
                "2 3 2",
                "1 3 5",
                "1 3"
            }));
        }

        [Fact]
        public void Example2()
        {
            Assert.Equal("6", Dijkstra.Solve(new string[] {
                "5 9",
                "1 2 4",
                "1 3 2",
                "2 3 2",
                "3 2 1",
                "2 4 2",
                "3 5 4",
                "5 4 1",
                "2 5 3",
                "3 4 4",
                "1 5"
            }));
        }

        [Fact]
        public void Example3()
        {
            Assert.Equal("-1", Dijkstra.Solve(new string[] {
                "3 3",
                "1 2 7",
                "1 3 5",
                "2 3 2",
                "3 2"
            }));
        }
    }
}
