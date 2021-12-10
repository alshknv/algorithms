using System;
using Xunit;
using _1_heavy_hitters;

namespace Tests
{
    public class HeavyHittersTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("0 1", HeavyHitters.Solve(3, 2, new int[][] {
                new int[] {3, 42},
                new int[] {8, 50001},
                new int[] {11, 230040},
                new int[] {8, 50000},
                new int[] {3, 40},
                new int[] {11, 230040} }, new int[] { 8, 3 }));
        }
    }
}
