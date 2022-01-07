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

        [Fact]
        public void Example2()
        {
            Assert.Equal("4", BubbleDetection.Solve(new string[] { "4 4", "AAACGCGTTGAACCCTCAAT", "GAATTGGAAACACGTTGAAT", "TGGAAACGCGTTGAACCCTC", "TGGAAACGCGTTGAACCCTC" }));
        }

        [Fact]
        public void Example3()
        {
            Assert.Equal("6", BubbleDetection.Solve(new string[] { "3 4", "AAABBBA", "AABCCBA", "AAABBCBA" }));
        }

        [Fact]
        public void Example4()
        {
            Assert.Equal("0", BubbleDetection.Solve(new string[] { "3 1", "AABBAABB" }));
        }

        [Fact]
        public void Example5()
        {
            Assert.Equal("0", BubbleDetection.Solve(new string[] { "3 6", "AABACBADAA" }));
        }

        [Fact]
        public void Example6()
        {
            Assert.Equal("3", BubbleDetection.Solve(new string[] { "3 3", "AABBA", "AACBA", "AADBA" }));
        }

        [Fact]
        public void Example7()
        {
            Assert.Equal("1", BubbleDetection.Solve(new string[] { "3 3", "AACG", "AAGG", "ACGT", "AGGT", "CGTT", "GCAA", "GGTT", "GTTG", "TGCA", "TTGC" }));
        }

        [Fact]
        public void Example8()
        {
            Assert.Equal("2", BubbleDetection.Solve(new string[] { "3 3", "AACG", "CGTT", "TTAA", "AATG", "TGTT", "ACCT", "CCTT" }));
        }

        [Fact]
        public void Example9()
        {
            Assert.Equal("2", BubbleDetection.Solve(new string[] { "4 6", "ATCCTAG", "TCCTAGA", "ATCGTCA", "CGTCAGA", "CGTTTCA", "TTTCAGA" }));
        }

        [Fact]
        public void Example10()
        {
            Assert.Equal("3", BubbleDetection.Solve(new string[] { "3 4", "ATGCAG", "ATCGCA", "ATACGC" }));
        }
    }
}