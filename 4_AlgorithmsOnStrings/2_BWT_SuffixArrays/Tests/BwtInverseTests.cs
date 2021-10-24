using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using Xunit;
using _2_bwtinverse;

namespace Tests
{
    public class BwtInverseTests
    {
        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests2/sample1";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = BwtInverse.Solve(input[0]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests2/sample2";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = BwtInverse.Solve(input[0]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void Test1()
        {
            Assert.Equal("AGACATA$", BwtInverse.Solve("ATG$CAAA"));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("BANANA$", BwtInverse.Solve("ANNB$AA"));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal("PANAMABANANAS$", BwtInverse.Solve("SMNPBNNAAAAA$A"));
        }

        [Fact]
        public void Perfomance()
        {
            var sb = new StringBuilder();
            var rnd = new Random();
            var chars = new Dictionary<int, char>() {
                {0, 'A'}, {1, 'C'}, {2, 'G'}, {3, 'T'}
            };
            var endPlace = rnd.Next(1000000);
            for (int i = 0; i < 1000000; i++)
            {
                if (i == endPlace)
                    sb.Append('$');
                else
                    sb.Append(chars[rnd.Next(10000) % 4]);
            }
            var watch = new Stopwatch();
            watch.Start();
            var result = BwtInverse.Solve(sb.ToString());
            watch.Stop();
            Assert.NotEmpty(result);
            Assert.True(watch.ElapsedMilliseconds <= 3000);
        }
    }
}