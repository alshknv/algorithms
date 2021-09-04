using System.IO;
using System.Linq;
using Xunit;
using _1_tree_traversals;

namespace Tests
{
    public class TreeTraversalsTests
    {
        [Fact]
        public void Examples()
        {
            Assert.Collection(
                TreeTraversals.Solve(new string[]{
                    "4 1 2",
                    "2 3 4",
                    "5 -1 -1",
                    "1 -1 -1",
                    "3 -1 -1"
                }),
                l1 => Assert.Equal("1 2 3 4 5", l1),
                l2 => Assert.Equal("4 2 1 3 5", l2),
                l3 => Assert.Equal("1 3 2 5 4", l3)
            );
            Assert.Collection(
                TreeTraversals.Solve(new string[]{
                    "0 7 2",
                    "10 -1 -1",
                    "20 -1 6",
                    "30 8 9",
                    "40 3 -1",
                    "50 -1 -1",
                    "60 1 -1",
                    "70 5 4",
                    "80 -1 -1",
                    "90 -1 -1"
                }),
                l1 => Assert.Equal("50 70 80 30 90 40 0 20 10 60", l1),
                l2 => Assert.Equal("0 70 50 40 30 80 90 20 60 10", l2),
                l3 => Assert.Equal("50 80 90 30 40 70 10 60 20 0", l3)
            );
        }

        [Fact]
        public void FromFile()
        {
            var testfiles = Directory.EnumerateFiles("../../../tests1");
            foreach (var tf in testfiles)
            {
                if (!tf.Contains(".a"))
                {
                    var lines = File.ReadAllLines(tf);
                    var answer = File.ReadAllLines($"{tf}.a");
                    Assert.Equal(answer, TreeTraversals.Solve(lines.Skip(1).ToArray()));
                }
            }
        }
    }
}
