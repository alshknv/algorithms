using System;
using Xunit;
using _7_maximum_salary;

namespace Tests
{
    public class MaximumSalaryTests
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("221", MaximumSalary.Solve("21 2"));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("99641", MaximumSalary.Solve("9 4 6 1 9"));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal("923923", MaximumSalary.Solve("23 39 92"));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal("992232", MaximumSalary.Solve("9 2 92 23"));
        }
    }
}
