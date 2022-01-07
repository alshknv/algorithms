using System;
using Xunit;
using _5_phi174_error_prone;

namespace Tests
{
    public class Phi174ErrorProneTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("ACGTTGCA", Phi174ErrorProne.Assemble(new string[] { "AACG", "AAGG", "ACGT", "CAAC", "CGTT", "GCAA", "GTTG", "TCCA", "TGCA", "TTGC" }));
        }
    }
}