using System.IO;
using System;
using Xunit;
using _1_kmp;

namespace Tests
{
    public class KmpTests
    {
        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests1/sample1";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = KnuthMorrisPratt.Solve(input[0], input[1]);
            Assert.Equal(answer.Length > 0 ? answer[0] : "", result);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests1/sample2";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = KnuthMorrisPratt.Solve(input[0], input[1]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void FileTest03()
        {
            const string tf = "../../../tests1/sample3";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = KnuthMorrisPratt.Solve(input[0], input[1]);
            Assert.Equal(answer[0], result);
        }
    }
}
