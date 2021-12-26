using System;
using Xunit;
using _4_phiX174_genome;

namespace Tests
{
    public class PhiX174GenomeTests
    {
        [Fact]
        public void Genome()
        {
            var chars = new char[] { 'A', 'C', 'G', 'T' };
            var rnd = new Random();
            const int N = 2000;
            const int readLen = 10;
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
            var result = PhiX174Genome.Assemble(reads);
            genome = genome.Substring(8) + genome.Substring(0, 8); // imitate circularity
            Assert.Equal(genome, result);
        }
    }
}