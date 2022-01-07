using System.IO;
using System;
using Xunit;
using _4_tip_removal;

namespace Tests
{
    public class TipRemovalTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("4", TipRemoval.Solve(new string[] { "AACG", "AAGG", "ACGT", "CAAC", "CGTT", "GCAA", "GTTG", "TCCA", "TGCA", "TTGC" }));
        }

        [Fact]
        public void Dataset1()
        {
            var input = File.ReadAllLines("../../../test4/tip1.in");
            Assert.Equal("3077", TipRemoval.Solve(input));
        }

        [Fact]
        public void Dataset2()
        {
            var input = File.ReadAllLines("../../../test4/tip2.in");
            Assert.Equal("3256", TipRemoval.Solve(input));
        }

        [Fact]
        public void Dataset3()
        {
            var input = File.ReadAllLines("../../../test4/tip3.in");
            Assert.Equal("3010", TipRemoval.Solve(input));
        }
    }
}