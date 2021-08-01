using System;
using Xunit;
using _2_majority_element;

namespace Tests
{
    public class MajorityElementTests
    {
        [Fact]
        public void Test1()
        {
            var result = MajorityElement.Solve("2 3 9 2 2");
            Assert.Equal("1", result);
        }

        [Fact]
        public void Test2()
        {
            var result = MajorityElement.Solve("1 2 3 4");
            Assert.Equal("0", result);
        }

        [Fact]
        public void Test3()
        {
            var result = MajorityElement.Solve("1 2 3 1");
            Assert.Equal("0", result);
        }

        [Fact]
        public void Test4()
        {
            var result = MajorityElement.Solve("2 3 2 9 2");
            Assert.Equal("1", result);
        }
    }
}
