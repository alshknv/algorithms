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
            Assert.Equal("AACGTTCG", ErrorFree.Assemble(new string[] { "AAC", "ACG", "GAA", "GTT", "TCG" }));
        }

        [Fact]
        public void Genome()
        {
            var chars = new char[] { 'A', 'C', 'G', 'T' };
            var rnd = new Random();
            const int N = 1500;
            const int readLen = 100;
            const int readShift = 1;
            var charArray = new char[N];
            for (int i = 0; i < N; i++)
            {
                charArray[i] = chars[rnd.Next(1000) % 4];
            }
            var genome = new string(charArray);
            var reads = new string[genome.Length / readShift];
            for (int i = 0; i < reads.Length; i++)
            {
                var startIdx = i * readShift;
                reads[i] = startIdx + readLen < genome.Length ?
                    genome.Substring(startIdx, readLen) :
                    genome[startIdx..] + genome[..(readLen - (genome.Length - startIdx))];
            }
            Assert.Equal(genome, ErrorFree.Assemble(reads));
        }
    }
}
