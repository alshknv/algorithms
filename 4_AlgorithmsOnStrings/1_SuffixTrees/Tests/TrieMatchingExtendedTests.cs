using System;
using System.IO;
using System.Linq;
using Xunit;
using _3_trie_matching_extended;

namespace Tests
{
    public class TrieMatchingExtendedTests
    {
        private string MakeString(Random rnd, int length)
        {
            var line = "";
            for (int i = 0; i < length; i++)
            {
                var val = rnd.Next(1000) % 4;
                switch (val)
                {
                    case 0: line += "A"; break;
                    case 1: line += "C"; break;
                    case 2: line += "G"; break;
                    case 3: line += "T"; break;
                }
            }
            return line;
        }

        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests3/sample1";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = TrieMatchingExtended.Solve(lines[0], lines.Skip(2).ToArray());
            Assert.Equal(answer.Length > 0 ? answer[0] : "", result);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests3/sample2";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = TrieMatchingExtended.Solve(lines[0], lines.Skip(2).ToArray());
            Assert.Equal(answer.Length > 0 ? answer[0] : "", result);
        }

        [Fact]
        public void Test1()
        {
            var lines = new string[] {"ATTGTTTTCTCGAGCGC",
                                        "7",
                                        "TAATA",
                                        "AAGTGAGCAG",
                                        "GCTCACATA",
                                        "CCAC",
                                        "C",
                                        "GCC",
                                        "TGTTCTTA"};
            Assert.Equal("8 10 14 16", TrieMatchingExtended.Solve(lines[0], lines.Skip(2).ToArray()));
        }

        [Fact]
        public void StressBrute()
        {
            var rnd = new Random();
            for (int k = 0; k < 100; k++)
            {
                var text = MakeString(rnd, 30);
                var patterns = new string[10];
                for (int i = 0; i < 10; i++)
                {
                    patterns[i] = MakeString(rnd, rnd.Next(10) + 1);
                }
                var brute = TrieMatchingExtended.BruteForce(text, patterns);
                var answer = TrieMatchingExtended.Solve(text, patterns);
                Assert.Equal(brute, answer);
            }
        }
    }
}