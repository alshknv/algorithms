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
    }
}
