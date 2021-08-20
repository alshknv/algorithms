using System;
using Xunit;
using _5_max_sliding_window;

namespace Tests
{
    public class MaxSlidingWindowTests
    {
        [Fact]
        public void Example()
        {
            Assert.Equal("7 7 5 6 6", MaxSlidingWindow.Solve("2 7 3 1 5 2 6 2", "4"));
        }

        [Fact]
        public void NaiveStress()
        {
            var random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var data = new int[10000];
                for (int j = 0; j < 10000; j++)
                {
                    data[j] = random.Next(10000);
                }
                var stringData = string.Join(" ", data);
                var m = (random.Next(1000) + 1).ToString();
                Assert.Equal(MaxSlidingWindow.Naive(stringData, m), MaxSlidingWindow.Solve(stringData, m));
            }
        }
    }
}