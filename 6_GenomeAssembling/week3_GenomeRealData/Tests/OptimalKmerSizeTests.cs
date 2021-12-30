using System;
using Xunit;
using _2_optimal_kmer_size;

namespace Tests
{
    public class OptimalKmerSizeTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("3", OptimalKmerSize.Solve(new string[] { "AACG", "ACGT", "CAAC", "GTTG", "TGCA" }));
        }

        [Fact]
        public void Example2()
        {
            Assert.Equal("5", OptimalKmerSize.Solve(new string[] { "ATGCCGTATG", "GCCGTATGGA", "GTATGGACAA", "GACAACGACT", "ACGACTATGC" }));
        }
    }
}
