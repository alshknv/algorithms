using System;
using System.Linq;
using System.IO;
using _5_non_shared_substring;
using Xunit;

namespace Tests
{
    public class NonSharedSubstringTests
    {
        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests5/sample1";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = NonSharedSubstring.Solve(lines[0], lines[1]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests5/sample2";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = NonSharedSubstring.Solve(lines[0], lines[1]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void FileTest03()
        {
            const string tf = "../../../tests5/sample3";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = NonSharedSubstring.Solve(lines[0], lines[1]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void FileTest04()
        {
            const string tf = "../../../tests5/sample4";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = NonSharedSubstring.Solve(lines[0], lines[1]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void Test1()
        {
            Assert.Equal("A", NonSharedSubstring.Solve("TGGATAATAC", "CCCTGCTCTT"));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("AAC", NonSharedSubstring.Solve("ACAAC", "CACAA"));

        }

        [Fact]
        public void Test3()
        {
            Assert.Equal("AAA", NonSharedSubstring.Solve("AAACAAAA", "ACAAGATA"));

        }
    }
}