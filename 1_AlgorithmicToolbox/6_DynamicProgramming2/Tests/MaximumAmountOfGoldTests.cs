using System;
using Xunit;
using _1_maximum_amount_of_gold;

namespace Tests
{
    public class MaximumAmountOfGoldTests
    {
        [Fact]
        public void Simple()
        {
            Assert.Equal("9", MaximumAmountOfGold.Solve("10 3", "1 4 8"));
        }

        [Fact]
        public void Stress()
        {
            var random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var bars = new int[5];
                var capacity = random.Next(900) + 100;
                for (int j = 0; j < 5; j++)
                {
                    bars[j] = random.Next(90) + 10;
                }
                var iterative = MaximumAmountOfGold.Iterative(bars, capacity);
                var recursive = MaximumAmountOfGold.Iterative(bars, capacity);
                var brute = MaximumAmountOfGold.BruteForce(bars, capacity, 0);
                Assert.Equal(iterative, recursive);
                Assert.Equal(iterative, brute);
                Assert.Equal(brute, recursive);
            }
        }
    }
}
