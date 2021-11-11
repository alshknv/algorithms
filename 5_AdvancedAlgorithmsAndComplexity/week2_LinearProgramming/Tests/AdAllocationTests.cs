using System.Linq;
using System.IO;
using System;
using Xunit;
using _3_ad_allocation;

namespace Tests
{
    public class AdAllocationTests
    {
        [Fact]
        public void BoundedSolution()
        {
            Assert.Collection(AdAllocation.Solve(new string[] { "3 2", "-1 -1", "1 0", "0 1", "-1 2 2", "-1 2" }),
                l1 => Assert.Equal("Bounded solution", l1),
                l2 => Assert.Equal("0.000000000000000000 2.000000000000000000", l2));
        }

        [Fact]
        public void Infinity()
        {
            Assert.Collection(AdAllocation.Solve(new string[] { "1 3", "0 0 1", "3", "1 1 1" }),
                l1 => Assert.Equal("Infinity", l1));
        }

        [Fact]
        public void NoSolution()
        {
            Assert.Collection(AdAllocation.Solve(new string[] { "2 2", "1 1", "-1 -1", "1 -2", "1 1" }),
                l1 => Assert.Equal("No solution", l1));
        }

        [Fact]
        public void FileTests()
        {
            var files = Directory.EnumerateFiles("../../../tests3").Where(f => !f.Contains(".a")).ToArray();
            Array.Sort(files);
            foreach (var file in files)
            {
                var input = File.ReadAllLines(file);
                var answer = File.ReadAllLines($"{file}.a");
                if (answer.Length > 1)
                {
                    answer[1] = string.Join(" ", answer[1].Split(' ').Select(x => x.IndexOf('.') >= 0 ? x.Substring(0, x.IndexOf('.') + 15) : x));
                }
                var result = AdAllocation.Solve(input);
                if (result.Length > 1)
                {
                    result[1] = string.Join(" ", answer[1].Split(' ').Select(x => x.IndexOf('.') >= 0 ? x.Substring(0, x.IndexOf('.') + 15) : x));
                }
                Assert.Equal(answer, result);
            }
        }
    }
}