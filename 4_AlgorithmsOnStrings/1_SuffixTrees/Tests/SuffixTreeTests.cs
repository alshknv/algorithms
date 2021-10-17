using System;
using System.Linq;
using System.IO;
using _4_suffix_tree;
using Xunit;

namespace Tests
{
    public class SuffixTreeTests
    {
        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests4/sample1";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = SuffixTree.Solve(lines[0]);
            Array.Sort(answer);
            Array.Sort(result);
            Assert.Equal(answer, result);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests4/sample2";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = SuffixTree.Solve(lines[0]);
            Array.Sort(answer);
            Array.Sort(result);
            Assert.Equal(answer, result);
        }

        [Fact]
        public void FileTest03()
        {
            const string tf = "../../../tests4/sample3";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = SuffixTree.Solve(lines[0]);
            Array.Sort(answer);
            Array.Sort(result);
            Assert.Equal(answer, result);
        }
    }
}