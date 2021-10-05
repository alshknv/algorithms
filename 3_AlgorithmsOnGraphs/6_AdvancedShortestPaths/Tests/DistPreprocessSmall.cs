using System.IO;
using System.Linq;
using _3_dist_preprocess_small;
using Xunit;

namespace Tests
{
    public class DistPreprocessSmallTests
    {
        private string[] SolveFromFile(string[] lines)
        {
            var nm = lines[0].AsIntArray();
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
            Assert.Collection(DistPreprocessSmall.ProcessQueries(new string[] {"6 5"}),
                l1 => Assert.Equal("8", l1));
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
    }
}