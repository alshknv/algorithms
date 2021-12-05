using _1_circuit_design;
using Xunit;

namespace Tests
{
    public class CircuitDesignTests
    {
        [Fact]
        public void Satisfiable()
        {
            Assert.Collection(CircuitDesign.Solve(new string[] {
                "3 3",
                "1 -3",
                "-1 2",
                "-2 -3"
            }),
            l1 => Assert.Equal("SATISFIABLE", l1),
            l2 => Assert.Equal("1 2 -3", l2));
        }

        [Fact]
        public void Unatisfiable()
        {
            Assert.Collection(CircuitDesign.Solve(new string[] {
                "1 2",
                "1 1",
                "-1 -1"
            }),
            l1 => Assert.Equal("UNSATISFIABLE", l1));
        }
    }
}
