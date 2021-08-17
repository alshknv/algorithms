using Xunit;
using _3_network_simulation;

namespace Tests
{
    public class NetworkSimulationTests
    {
        [Fact]
        public void Examples()
        {
            Assert.Empty(NetworkSimulation.Solve("1 0", new string[0]));
            Assert.Collection(NetworkSimulation.Solve("1 1", new string[1] { "0 0" }),
                l1 => Assert.Equal("0", l1));
            Assert.Collection(NetworkSimulation.Solve("1 2", new string[2] { "0 1", "0 1" }),
                l1 => Assert.Equal("0", l1),
                l2 => Assert.Equal("-1", l2));
            Assert.Collection(NetworkSimulation.Solve("1 2", new string[2] { "0 1", "1 1" }),
                l1 => Assert.Equal("0", l1),
                l2 => Assert.Equal("1", l2));
        }

        [Fact]
        public void Complex()
        {
            Assert.Collection(NetworkSimulation.Solve("1 3", new string[3] { "0 1", "1 3", "4 2" }),
                l1 => Assert.Equal("0", l1),
                l2 => Assert.Equal("1", l2),
                l3 => Assert.Equal("4", l3));
            Assert.Collection(NetworkSimulation.Solve("3 6", new string[6] { "0 2", "1 2", "2 2", "3 2", "4 2", "5 2" }),
                l1 => Assert.Equal("0", l1),
                l2 => Assert.Equal("2", l2),
                l3 => Assert.Equal("4", l3),
                l4 => Assert.Equal("6", l4),
                l5 => Assert.Equal("8", l5),
                l6 => Assert.Equal("-1", l6));
            Assert.Collection(NetworkSimulation.Solve("1 3", new string[3] { "0 417", "0 842", "0 919" }),
                l1 => Assert.Equal("0", l1),
                l2 => Assert.Equal("-1", l2),
                l3 => Assert.Equal("-1", l3));
        }
    }
}