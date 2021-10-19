using System.Diagnostics;
using System;
using System.Linq;
using System.IO;
using _4_suffix_tree;
using Xunit;

namespace Tests
{
    public class SuffixTreeTests
    {
        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests4/sample1";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = SuffixTree.Solve(lines[0]);
            Array.Sort(answer);
            Array.Sort(result);
            Assert.Equal(answer, result);
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests4/sample2";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = SuffixTree.Solve(lines[0]);
            Array.Sort(answer);
            Array.Sort(result);
            Assert.Equal(answer, result);
        }

        [Fact]
        public void FileTest03()
        {
            const string tf = "../../../tests4/sample3";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = SuffixTree.Solve(lines[0]);
            Array.Sort(answer);
            Array.Sort(result);
            Assert.Equal(answer, result);
        }

        [Fact]

        public void Test1()
        {
            var answer = new string[] { "$", "$", "A$", "A$", "CAA$", "A$", "CAA$", "CA", "CA", "A" };
            var result = SuffixTree.Solve("ACACAA$");
            Array.Sort(answer);
            Array.Sort(result);
            Assert.Equal(answer, result);
        }


        [Fact]
        public void Perfomance()
        {
            var line = "";
            var rnd = new Random();
            for (int i = 0; i < 5000; i++)
            {
                var val = rnd.Next(1000) % 4;
                switch (val)
                {
                    case 0: line += "A"; break;
                    case 1: line += "C"; break;
                    case 2: line += "G"; break;
                    case 3: line += "T"; break;
                }
            }
            var watch = new Stopwatch();
            watch.Start();
            SuffixTree.Solve(line + "$");
            watch.Stop();
            Assert.True(watch.ElapsedMilliseconds <= 1500);
        }
    }
}