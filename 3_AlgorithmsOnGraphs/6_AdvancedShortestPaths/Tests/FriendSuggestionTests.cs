using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using _1_friend_suggestion;
using Xunit;

namespace Tests
{
    public class FriendSuggestionTests
    {
        private string[] SolveFromFile(string[] lines)
        {
            var nm = lines[0].AsIntArray();
            var edges = new string[nm[1]];
            for (int i = 0; i < nm[1]; i++)
            {
                edges[i] = lines[i + 1];
            }
            var q = int.Parse(lines[nm[1] + 1]);
            var queries = new string[q];
            for (int i = 0; i < q; i++)
            {
                queries[i] = lines[i + nm[1] + 2];
            }
            return FriendSuggestion.Solve(nm[0], edges, queries);
        }

        [Fact]
        public void Example1()
        {
            Assert.Collection(FriendSuggestion.Solve(4, new string[] { "1 2 1", "4 1 2", "2 3 2", "1 3 5" }, new string[] { "1 3" }),
                l1 => Assert.Equal("3", l1));
        }

        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests134/01";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            Assert.Equal(answer, SolveFromFile(lines));
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests134/02";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            Assert.Equal(answer, SolveFromFile(lines));
        }

        [Fact]
        public void FileTest03()
        {
            const string tf = "../../../tests134/03";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = SolveFromFile(lines);
            Assert.Equal(answer, result);
        }
    }
}
