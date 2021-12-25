using Xunit;
using _2_eulerian_cycle;

namespace Tests
{
    public class EulerianCycleTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Collection(EulerianCycle.Solve(new string[]{
                "3 4","2 3","2 2","1 2","3 1"
            }),
            l1 => Assert.Equal("1", l1),
            l2 => Assert.Equal("1 2 2 3", l2));
        }

        [Fact]
        public void Example2()
        {
            Assert.Collection(EulerianCycle.Solve(new string[]{
                "3 4","1 3","2 3","1 2","3 1"
            }),
            l1 => Assert.Equal("0", l1));
        }

        [Fact]
        public void Example3()
        {
            Assert.Collection(EulerianCycle.Solve(new string[]{
                "4 7","1 2","2 1","1 4","4 1","2 4","3 2","4 3"
            }),
            l1 => Assert.Equal("1", l1),
            l2 => Assert.Equal("1 4 3 2 4 1 2", l2));
        }

        [Fact]
        public void Example4()
        {
            Assert.Collection(EulerianCycle.Solve(new string[]{
                "4 7","2 3","3 4","1 4","3 1","4 2","2 3","4 2"
            }),
            l1 => Assert.Equal("1", l1),
            l2 => Assert.Equal("1 4 2 3 4 2 3", l2));
        }
    }
}