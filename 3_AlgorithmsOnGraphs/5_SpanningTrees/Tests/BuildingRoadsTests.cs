using _1_connecting_points;
using Xunit;

namespace Tests
{
    public class BuildingRoadsTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("3.000000000", BuildingRoads.Solve(new string[] {
                "0 0",
                "0 1",
                "1 0",
                "1 1"
            }));
        }

        [Fact]
        public void Example2()
        {
            Assert.Equal("7.064495102", BuildingRoads.Solve(new string[] {
                "0 0",
                "0 2",
                "1 1",
                "3 0",
                "3 2"
            }));
        }

        [Fact]
        public void Example3()
        {
            Assert.Equal("14.605551275", BuildingRoads.Solve(new string[] {
                "0 3",
                "0 0",
                "2 0",
                "4 3",
                "9 3",
                "9 2"
            }));
        }

        [Fact]
        public void OneNode()
        {
            Assert.Equal("0.000000000", BuildingRoads.Solve(new string[] {
                "0 0"
            }));
        }
    }
}
