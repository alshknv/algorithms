using Xunit;
using _1_flight_segments;

namespace Tests
{
    public class FlightSegmentsTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("2", FlightSegments.Solve(new string[] {
                "4 4",
                "1 2",
                "4 1",
                "2 3",
                "3 1",
                "2 4"
            }));
        }

        [Fact]
        public void Example2()
        {
            Assert.Equal("-1", FlightSegments.Solve(new string[] {
                "5 4",
                "5 2",
                "1 3",
                "3 4",
                "1 4",
                "3 5"
            }));
        }
    }
}
