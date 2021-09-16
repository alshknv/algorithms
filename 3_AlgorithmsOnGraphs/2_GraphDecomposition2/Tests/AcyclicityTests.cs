using System;
using Xunit;
using _1_acyclicity;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("1", Acyclicity.Solve(new string[]{
                "4 5",
                "2 1",
                "4 3",
                "1 4",
                "2 4",
                "3 2"
            }));
        }

        [Fact]
        public void Example2() {
            Assert.Equal("1", Acyclicity.Solve(new string[]{
                "2 2",
                "1 2",
                "2 1"
            }));
        }

        [Fact]
        public void Example3()
        {
            Assert.Equal("1", Acyclicity.Solve(new string[]{
                "4 4",
                "1 2",
                "4 1",
                "2 3",
                "3 1"
            }));
        }

        [Fact]
        public void Example4()
        {
            Assert.Equal("0", Acyclicity.Solve(new string[]{
                "5 7",
                "1 2",
                "2 3",
                "1 3",
                "3 4",
                "1 4",
                "2 5",
                "3 5"
            }));
        }
    }
}
