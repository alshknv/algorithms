using System;
using System.IO;
using System.Linq;
using Xunit;
using _3_trie_matching_extended;

namespace Tests
{
    public class TrieMatchingExtendedTests
    {
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
    }
}