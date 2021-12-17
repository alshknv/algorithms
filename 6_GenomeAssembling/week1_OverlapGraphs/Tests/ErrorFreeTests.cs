using System;
using Xunit;
using _1_phiX174_error_free;

namespace Tests
{
    public class ErrorFreeTests
    {
        [Fact]
        public void Simple()
        {
            Assert.Equal("ACGTTCGA", ErrorFree.Assemble(new string[] { "AAC", "ACG", "GAA", "GTT", "TCG" }));
        }

        [Fact]
        public void Genome()
        {
            var chars = new char[] { 'A', 'C', 'G', 'T' };
            var rnd = new Random();
            const int N = 1500;
            const int readLen = 100;
            const int readShift = 20;
            var charArray = new char[N];
            for (int i = 0; i < N; i++)
            {
                charArray[i] = chars[rnd.Next(1000) % 4];
            }
            var genome = new string(charArray);
            var reads = new string[(genome.Length - (readLen - readShift)) / readShift];
            for (int i = 0; i < reads.Length; i++)
            {
                reads[i] = genome.Substring(i * readShift, readLen);
            }
            Assert.Equal(genome, ErrorFree.Assemble(reads));
        }
    }
}
