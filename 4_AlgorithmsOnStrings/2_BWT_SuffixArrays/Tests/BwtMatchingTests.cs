using System.IO;
using System.Linq;
using Xunit;
using _3_bwmatching;

namespace Tests
{
    public class BwtMatchingTests
    {
        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests3/sample1";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = BwtMatching.Solve(input[0], input[2].Split(' ').ToArray());
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests3/sample2";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = BwtMatching.Solve(input[0], input[2].Split(' ').ToArray());
            Assert.Equal(answer[0], result);
        }

        [Fact]
        public void FileTest03()
        {
            const string tf = "../../../tests3/sample3";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = BwtMatching.Solve(input[0], input[2].Split(' ').ToArray());
            Assert.Equal(answer[0], result);
        }
    }
}