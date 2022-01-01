using System;
using Xunit;
using _3_bubble_detection;

namespace Tests
{
    public class BubbleDetectionTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("1", BubbleDetection.Solve(new string[] { "3 3", "AACG", "AAGG", "ACGT", "AGGT", "CGTT", "GCAA", "GGTT", "GTTG", "TGCA", "TTGC" }));
        }
    }
}