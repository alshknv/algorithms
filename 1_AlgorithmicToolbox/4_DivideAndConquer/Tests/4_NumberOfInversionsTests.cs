using System;
using Xunit;
using _4_number_of_inversions;

namespace Tests
{
    public class NumberOfInversionsTests
    {
        [Fact]
        public void Test1()
        {
            var result = NumberOfInversions.Solve("2 3 9 2 9");
            Assert.Equal("2", result);
        }

        [Fact]
        public void Test2()
        {
            var result = NumberOfInversions.Solve("2 3 5 2 4 9");
            Assert.Equal("3", result);
        }

        [Fact]
        public void Test3()
        {
            var result = NumberOfInversions.Solve("9 8 7 3 2 1");
            Assert.Equal("15", result);
        }

        [Fact]
        public void Test4()
        {
            var result = NumberOfInversions.Solve("1 0 2 3 4 5");
            Assert.Equal("1", result);
        }

        [Fact]
        public void StressNaive()
        {
            var random = new Random();
            for (int i = 0; i < 10000; i++)
            {
                var values = new int[10];
                for (int j = 0; j < 10; j++)
                {
                    values[j] = random.Next(3) - 1;
                }
                var input = string.Join(" ", values);
                Assert.Equal(NumberOfInversions.Naive(input), NumberOfInversions.Solve(input));
            }
        }
    }
}
