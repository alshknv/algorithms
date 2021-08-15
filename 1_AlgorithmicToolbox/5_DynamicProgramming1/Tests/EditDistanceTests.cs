using Xunit;
using _3_edit_distance;

namespace Tests
{
    public class EditDistanceTests
    {
        [Fact]
        public void BreadReally()
        {
            Assert.Equal("4", EditDistance.Solve("bread", "really"));
        }

        [Fact]
        public void EqualStrings()
        {
            Assert.Equal("0", EditDistance.Solve("abababab", "abababab"));
        }

        [Fact]
        public void ShortPorts()
        {
            Assert.Equal("3", EditDistance.Solve("short", "ports"));
        }

        [Fact]
        public void EditingDistance()
        {
            Assert.Equal("5", EditDistance.Solve("editing", "distance"));
        }
    }
}