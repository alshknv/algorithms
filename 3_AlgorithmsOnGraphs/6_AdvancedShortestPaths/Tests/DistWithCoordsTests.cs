using System.IO;
using System.Linq;
using Xunit;
using _2_dist_with_coords;

namespace Tests
{
    public class DistWithCoordsTests
    {
        private string[] SolveFromFile(string[] lines)
        {
            var nm = lines[0].AsIntArray();
            var vertices = new string[nm[0]];
            for (int i = 0; i < nm[0]; i++)
            {
                vertices[i] = lines[i + 1];
            }
            var edges = new string[nm[1]];
            for (int i = 0; i < nm[1]; i++)
            {
                edges[i] = lines[i + nm[0] + 1];
            }
            var q = int.Parse(lines[nm[0] + nm[1] + 1]);
            var queries = new string[q];
            for (int i = 0; i < q; i++)
            {
                queries[i] = lines[i + nm[0] + nm[1] + 2];
            }
            return DistWithCoords.Solve(vertices, edges, queries);
        }

        [Fact]
        public void Example1()
        {
            Assert.Collection(
            DistWithCoords.Solve(new string[] { "0 0", "1 1", "3 4", "8 8", "5 6", "7 7", "10 6", "10 8", "11 9" },
            new string[] { "1 4 12", "4 7 5", "7 9 4", "9 8 2", "1 2 2", "2 3 4", "3 5 3", "5 6 3", "6 8 4" },
            new string[] { "1 8" }),
            l1 => Assert.Equal("16", l1));
        }

        [Fact]
        public void FileTest01()
        {
            const string tf = "../../../tests2/01";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            Assert.Equal(answer, SolveFromFile(lines));
        }

        [Fact]
        public void FileTest02()
        {
            const string tf = "../../../tests2/02";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            Assert.Equal(answer, SolveFromFile(lines));
        }

        [Fact]
        public void FileTest03()
        {
            const string tf = "../../../tests2/03";
            var lines = File.ReadAllLines(tf);
            var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
            var result = SolveFromFile(lines);
            for (int i = 0; i < answer.Length; i++)
            {
                if (answer[i] != result[i])
                {

                }
            }
            Assert.Equal(answer, result);
        }
    }
}