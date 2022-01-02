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
    }
}