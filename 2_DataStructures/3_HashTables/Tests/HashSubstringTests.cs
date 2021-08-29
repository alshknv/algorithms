using System.Diagnostics;
using System.Linq;
using System.IO;
using Xunit;
using _3_hash_substring;

namespace Tests
{
    public class HashSubstringTests
    {
        [Fact]
        public void SimpleExamples()
        {
            Assert.Equal("0 4", HashSubstring.Solve("aba", "abacaba"));
            Assert.Equal("4", HashSubstring.Solve("Test", "testTesttesT"));
            Assert.Equal("1 2 3", HashSubstring.Solve("aaaaa", "baaaaaaa"));
        }

        [Fact]
        public void FromFile()
        {
            var testfiles = Directory.EnumerateFiles("../../../tests2");
            foreach (var tf in testfiles)
            {
                if (!tf.Contains(".a"))
                {
                    var lines = File.ReadAllLines(tf);
                    var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).First();
                    Assert.Equal(answer, HashSubstring.Solve(lines[0], lines[1]));
                }
            }
        }

        [Fact]
        public void Runtime()
        {
            var text = "";
            var pattern = "";
            var p = "a";

            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i < 100000; i++)
                {
                    text += p;
                }
                if (p == "a") p = "b"; else p = "a";
            }
            for (int i = 0; i < 3; i++)
            {
                pattern += "b";
            }
            var watch = new Stopwatch();
            watch.Start();
            HashSubstring.Solve(pattern, text);
            watch.Stop();
            Assert.True(watch.ElapsedMilliseconds < 1500);
        }
    }
}