using System;
using System.IO;
using System.Linq;
using _3_dist_preprocess_small;
using Xunit;
using _1_friend_suggestion;

namespace Tests
{
    public class DistPreprocessSmallTests
    {
        private string[] SolveFromFile(string[] lines)
        {
            var nm = _3_dist_preprocess_small.Extensions.AsIntArray(lines[0]);
            var edges = new string[nm[1]];
            for (int i = 0; i < nm[1]; i++)
            {
                edges[i] = lines[i + 1];
            }
            DistPreprocessSmall.Preprocess(nm[0], edges);
            var q = int.Parse(lines[nm[1] + 1]);
            var queries = new string[q];
            for (int i = 0; i < q; i++)
            {
                queries[i] = lines[i + nm[1] + 2];
            }
            return DistPreprocessSmall.ProcessQueries(queries);
        }

        [Fact]
        public void Example1()
        {
            DistPreprocessSmall.Preprocess(6, new string[] {
                "2 6 2", "6 1 3","1 4 2","4 3 1","3 5 2"
            });
            Assert.Collection(DistPreprocessSmall.ProcessQueries(new string[] { "6 5" }),
                l1 => Assert.Equal("8", l1));
        }

        [Fact]
        public void Example2()
        {
            DistPreprocessSmall.Preprocess(9,
                new string[] { "1 4 12", "4 7 5", "7 9 4", "9 8 2", "1 2 2", "2 3 4", "3 5 3", "5 6 3", "6 8 4" });
            Assert.Collection(DistPreprocessSmall.ProcessQueries(new string[] { "1 8" }),
                l1 => Assert.Equal("16", l1));
        }

        [Fact]
        public void Loop()
        {
            DistPreprocessSmall.Preprocess(3,
                new string[] { "1 2 1", "2 3 2", "3 1 3" });
            Assert.Collection(DistPreprocessSmall.ProcessQueries(new string[] { "1 2", "2 3", "3 1", "1 3", "2 1", "3 2" }),
                l1 => Assert.Equal("1", l1),
                l2 => Assert.Equal("2", l2),
                l3 => Assert.Equal("3", l3),
                l4 => Assert.Equal("3", l4),
                l5 => Assert.Equal("5", l5),
                l6 => Assert.Equal("4", l6));
        }

        [Fact]
        public void MultiEdge()
        {
            DistPreprocessSmall.Preprocess(3,
                new string[] { "1 2 1", "2 3 2", "3 1 3", "2 3 1" });
            Assert.Collection(DistPreprocessSmall.ProcessQueries(new string[] { "1 2", "2 3", "3 1", "1 3", "2 1", "3 2" }),
                l1 => Assert.Equal("1", l1),
                l2 => Assert.Equal("1", l2),
                l3 => Assert.Equal("3", l3),
                l4 => Assert.Equal("2", l4),
                l5 => Assert.Equal("4", l5),
                l6 => Assert.Equal("4", l6));
        }

        [Fact]
        public void SelfLoop()
        {
            DistPreprocessSmall.Preprocess(3,
                new string[] { "1 2 1", "2 3 2", "3 1 3", "2 3 1", "1 1 1" });
            Assert.Collection(DistPreprocessSmall.ProcessQueries(new string[] { "1 2", "2 3", "3 1", "1 3", "2 1", "3 2" }),
                l1 => Assert.Equal("1", l1),
                l2 => Assert.Equal("1", l2),
                l3 => Assert.Equal("3", l3),
                l4 => Assert.Equal("2", l4),
                l5 => Assert.Equal("4", l5),
                l6 => Assert.Equal("4", l6));
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
            Assert.Equal(answer, SolveFromFile(lines));
        }

        [Fact]
        public void AgainstDijkstra()
        {
            const string tf = "../../../tests134/03";
            var lines = File.ReadAllLines(tf);
            var nm = _3_dist_preprocess_small.Extensions.AsIntArray(lines[0]);
            var edges = new string[nm[1]];
            for (int i = 0; i < nm[1]; i++)
            {
                edges[i] = lines[i + 1];
            }
            DistPreprocessSmall.Preprocess(nm[0], edges);
            var rnd = new Random();
            var queries = new string[1000];
            for (int i = 0; i < queries.Length; i++)
            {
                queries[i] = $"{rnd.Next(nm[0] * 100) % nm[0] + 1} {rnd.Next(nm[0] * 100) % nm[0] + 1}";
            }
            Assert.Equal(FriendSuggestion.Solve(nm[0], edges, queries),
            DistPreprocessSmall.ProcessQueries(queries));
        }
    }
}