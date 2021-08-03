using System.Diagnostics;
using System.Linq;
using System;
using Xunit;
using _3_improving_quicksort;

namespace Tests
{
    public class ImprovingQuicksortTests
    {
        [Fact]
        public void Test1()
        {
            var result = ImprovingQuicksort.Solve("2 3 9 2 2");
            Assert.Equal("2 2 2 3 9", result);
        }

        [Fact]
        public void Test2()
        {
            var result = ImprovingQuicksort.Solve("2 8 3 6 1 2 9 2 0 2");
            Assert.Equal("0 1 2 2 2 2 3 6 8 9", result);
        }

        [Fact]
        public void Stress()
        {
            var input = $"0 2 1 {string.Join(" ", Enumerable.Repeat(3, 100000).ToArray())} 5 4";
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var result = ImprovingQuicksort.Solve(input);
            watch.Stop();
            Assert.Equal($"0 1 2 {string.Join(" ", Enumerable.Repeat(3, 100000).ToArray())} 4 5", result);
            Assert.True(watch.ElapsedMilliseconds < 500);
        }
    }
}
