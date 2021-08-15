using System;
using Xunit;
using _2_partitioning_souvenirs;

namespace Tests
{
    public class PertitioningSouvenirsTests
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("0", PartitioningSouvenirs.Solve("3 3 3 3"));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("0", PartitioningSouvenirs.Solve("40"));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal("1", PartitioningSouvenirs.Solve("17 59 34 57 17 23 67 1 18 2 59"));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal("1", PartitioningSouvenirs.Solve("1 2 3 4 5 5 7 7 8 10 12 19 25"));
            Assert.Equal("1", PartitioningSouvenirs.Solve("1 2 3 4 4 5 5 7 8 10 12 19 25"));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal("1", PartitioningSouvenirs.Solve("1 1 1"));
        }
    }
}
