using System.IO;
using System;
using Xunit;
using _2_suffix_array_long;


namespace Tests
{
    public class SuffixArrayLongTests
    {
        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests2/sample1";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = SuffixArrayLong.Solve(input[0]);
            Assert.Equal(answer.Length > 0 ? answer[0] : "", result);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests2/sample2";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = SuffixArrayLong.Solve(input[0]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void FileTest03()
        {
            const string tf = "../../../tests2/sample3";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = SuffixArrayLong.Solve(input[0]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void FileTest04()
        {
            const string tf = "../../../tests2/sample4";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = SuffixArrayLong.Solve(input[0]);
            Assert.Equal(answer[0], result);
        }
    }
}