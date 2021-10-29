using System;
using System.IO;
using System.Linq;
using Xunit;
using _3_suffix_array_matching;

namespace Tests
{
    public class SuffixArrayMatchingTests
    {
        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests3/sample1";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = SuffixArrayMatching.Solve(input[0], input[2]);
            var answerArray = answer[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            Array.Sort(answerArray);
            var resArray = result.Split(' ').Select(x => int.Parse(x)).ToArray();
            Array.Sort(resArray);
            Assert.Equal(answerArray, resArray);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests3/sample2";
            var input = File.ReadAllLines(tf);
            var result = SuffixArrayMatching.Solve(input[0], input[2]);
            Assert.Equal("", result);
        }

        [Fact]
        public void FileTest03()
        {
            const string tf = "../../../tests3/sample3";
            var input = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a");
            var result = SuffixArrayMatching.Solve(input[0], input[2]);
            var answerArray = answer[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            Array.Sort(answerArray);
            var resArray = result.Split(' ').Select(x => int.Parse(x)).ToArray();
            Array.Sort(resArray);
            Assert.Equal(answerArray, resArray);
        }

        [Fact]
        public void Test1()
        {
            var answer = "1 8 11 13 27 31";
            var result = SuffixArrayMatching.Solve("TCCTCTATGAGATCCTATTCTATGAAACCTTCAGACCAAAATTCTCCGGC", "CCT CAC GAG CAG ATC");
            var answerArray = answer.Split(' ').Select(x => int.Parse(x)).ToArray();
            Array.Sort(answerArray);
            var resArray = result.Split(' ').Select(x => int.Parse(x)).ToArray();
            Array.Sort(resArray);
            Assert.Equal(answerArray, resArray);
        }
    }
}