using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using _4_suffix_array;

namespace Tests
{
    public class SuffixArrayTests
    {
        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests4/sample1";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = SuffixArray.Solve(input[0]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests4/sample2";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = SuffixArray.Solve(input[0]);
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void FileTest03()
        {
            const string tf = "../../../tests4/sample3";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = SuffixArray.Solve(input[0]);
            Assert.Equal(answer[0], result);
        }
    }
}