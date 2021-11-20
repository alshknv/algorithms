using Xunit;
using _1_gsm_network;

namespace Tests
{
    public class GsmNetworkTests
    {
        [Fact]
        public void SAT()
        {
            var input = new string[] { "3 3", "1 2", "2 3", "1 3" };
            Assert.True(Minisat.SAT(GsmNetwork.Solve(input)));
        }

        [Fact]
        public void UNSAT()
        {
            var input = new string[] { "4 6", "1 2", "1 3", "1 4", "2 3", "2 4", "3 4" };
            Assert.True(Minisat.UNSAT(GsmNetwork.Solve(input)));
        }
    }
}
