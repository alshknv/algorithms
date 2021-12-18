using System;
using Xunit;
using _2_phiX174_error_prone;

namespace Tests
{
    public class ErrorProneTests
    {
        [Fact]
        public void Genome()
        {
            var chars = new char[] { 'A', 'C', 'G', 'T' };
            var rnd = new Random();
            const int N = 1500;
            const int readLen = 100;
            const int readShift = 5;
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
                var errorI = 1 + rnd.Next(1000) % (readLen - 2);
                var errorChar = chars[rnd.Next(1000) % 4];
                while (errorChar == reads[i][errorI])
                    errorChar = chars[rnd.Next(1000) % 4];
                reads[i] = $"{reads[i][0..errorI]}{errorChar}{reads[i][(errorI + 1)..]}";
            }
            Assert.Equal(genome, ErrorProne.Assemble(reads));
        }
    }
}