using System.IO;
using System.Linq;
using Xunit;
using _4_suffix_tree_from_array;

namespace Tests
{
    public class SuffixTreeFromArrayTests
    {
        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests4/sample1";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = SuffixTreeFromArray.Solve(input[0], input[1], input[2]);
            Assert.Equal(answer, result);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests4/sample2";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = SuffixTreeFromArray.Solve(input[0], input[1], input[2]);
            Assert.Equal(answer, result);
        }

        [Fact]
        public void FileTest03()
        {
            const string tf = "../../../tests4/sample3";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = SuffixTreeFromArray.Solve(input[0], input[1], input[2]);
            Assert.Equal(answer, result);
        }
    }
}