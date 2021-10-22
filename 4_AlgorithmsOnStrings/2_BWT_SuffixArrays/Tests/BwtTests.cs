using System.IO;
using System;
using Xunit;
using _1_bwt;

namespace Tests
{
    public class BwtTests
    {
        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests1/sample1";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = BurrowsWheelerTransform.Solve(input[0]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests1/sample2";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = BurrowsWheelerTransform.Solve(input[0]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void FileTest03()
        {
            const string tf = "../../../tests1/sample3";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = BurrowsWheelerTransform.Solve(input[0]);
            Assert.Equal(answer[0], result);
        }
    }
}
