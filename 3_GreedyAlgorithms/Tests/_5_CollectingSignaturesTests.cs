using System;
using System.Collections.Generic;
using Xunit;
using _5_collecting_signatures;

namespace Tests
{
    public class CollectingSignaturesTests
    {
        [Fact]
        public void Test1()
        {
            Assert.Collection(CollectingSignatures.Solve(new List<string>() {
                "3",
                "1 3",
                "2 5",
                "3 6"
            }),
            l1 => Assert.Equal("1", l1),
            l2 => Assert.Equal("3", l2));
        }

        [Fact]
        public void Test2()
        {
            Assert.Collection(CollectingSignatures.Solve(new List<string>() {
                "4",
                "4 7",
                "1 3",
                "2 5",
                "5 6"
            }),
            l1 => Assert.Equal("2", l1),
            l2 => Assert.Equal("3 6", l2));
        }
    }
}
