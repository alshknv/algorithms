using System.IO;
using System;
using Xunit;
using _2_bwtinverse;

namespace Tests
{
    public class BwtReverseTests
    {
        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests2/sample1";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = BwtInverse.Solve(input[0]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests2/sample2";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = BwtInverse.Solve(input[0]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void Test1()
        {
            Assert.Equal("AGACATA$", BwtInverse.Solve("ATG$CAAA"));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("BANANA$", BwtInverse.Solve("ANNB$AA"));
        }
    }
}