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

        [Fact]
        public void Test1()
        {
            var result = SuffixTreeFromArray.Solve("ATAAATG$", "7 2 3 0 4 6 1 5", "0 2 1 2 0 0 1");
            Assert.Collection(result,
            l => Assert.Equal("ATAAATG$", l),
            l => Assert.Equal("7 8", l),
            l => Assert.Equal("4 5", l),
            l => Assert.Equal("4 5", l),
            l => Assert.Equal("4 8", l),
            l => Assert.Equal("5 8", l),
            l => Assert.Equal("5 6", l),
            l => Assert.Equal("2 8", l),
            l => Assert.Equal("6 8", l),
            l => Assert.Equal("6 8", l),
            l => Assert.Equal("5 6", l),
            l => Assert.Equal("2 8", l),
            l => Assert.Equal("6 8", l));
        }
    }
}