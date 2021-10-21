using System;
using System.Linq;
using System.IO;
using Xunit;
using _1_trie;

namespace Tests
{
    public class TrieTests
    {
        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests1/sample1";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = Trie.Solve(lines.Skip(1).ToArray());
            Array.Sort(answer);
            Array.Sort(result);
            Assert.Equal(answer, result);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests1/sample2";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = Trie.Solve(lines.Skip(1).ToArray());
            Array.Sort(answer);
            Array.Sort(result);
            Assert.Equal(answer, result);
        }

        [Fact]
        public void FileTest03()
        {
            const string tf = "../../../tests1/sample3";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = Trie.Solve(lines.Skip(1).ToArray());
            Array.Sort(answer);
            Array.Sort(result);
            Assert.Equal(answer, result);
        }
    }
}
