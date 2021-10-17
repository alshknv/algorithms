using System.Diagnostics;
using System;
using System.IO;
using System.Linq;
using Xunit;
using _2_trie_matching;

namespace Tests
{
    public class TrieMatchingTests
    {
        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests2/sample1";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = TrieMatching.Solve(lines[0], lines.Skip(2).ToArray());
            Assert.Equal(answer.Length > 0 ? answer[0] : "", result);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests2/sample2";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = TrieMatching.Solve(lines[0], lines.Skip(2).ToArray());
            Assert.Equal(answer.Length > 0 ? answer[0] : "", result);
        }

        [Fact]
        public void FileTest03()
        {
            const string tf = "../../../tests2/sample3";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = TrieMatching.Solve(lines[0], lines.Skip(2).ToArray());
            Assert.Equal(answer.Length > 0 ? answer[0] : "", result);
        }

        [Fact]
        public void LongStringPerfomance()
        {
            var line = new string('A', 10000);
            var watch = new Stopwatch();
            watch.Start();
            TrieMatching.Solve(line, new string[] { "A" });
            watch.Stop();
            Assert.True(watch.ElapsedMilliseconds <= 500);
        }
    }
}