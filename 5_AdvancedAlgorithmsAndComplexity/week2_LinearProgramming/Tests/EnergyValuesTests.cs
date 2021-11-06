using System;
using Xunit;
using _1_energy_values;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void EmptyMatrix()
        {
            Assert.Equal("", EnergyValues.Solve(new string[] { }));
        }

        [Fact]
        public void Example1()
        {
            Assert.Equal("1.00000 5.00000 4.00000 3.00000", EnergyValues.Solve(new string[] { "1 0 0 0 1", "0 1 0 0 5", "0 0 1 0 4", "0 0 0 1 3" }));
        }

        [Fact]
        public void Example2()
        {
            Assert.Equal("2.00000 1.00000", EnergyValues.Solve(new string[] { "1 1 3", "2 3 7" }));
        }

        [Fact]
        public void Example3()
        {
            Assert.Equal("0.20000 0.40000", EnergyValues.Solve(new string[] { "5 -5 -1", "-1 -2 -1" }));
        }
    }
}
