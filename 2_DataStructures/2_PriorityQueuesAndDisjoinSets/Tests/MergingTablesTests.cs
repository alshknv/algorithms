using System.IO;
using System.Linq;
using Xunit;
using _3_merging_tables;

namespace Tests
{
    public class MergingTablesTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Collection(MergingTables.Solve("5 5", "1 1 1 1 1", new string[] { "3 5", "2 4", "1 4", "5 4", "5 3" }),
                l1 => Assert.Equal("2", l1),
                l2 => Assert.Equal("2", l2),
                l3 => Assert.Equal("3", l3),
                l4 => Assert.Equal("5", l4),
                l5 => Assert.Equal("5", l5));
        }

        [Fact]
        public void Example2()
        {
            Assert.Collection(MergingTables.Solve("6 4", "10 0 5 0 3 3", new string[] { "6 6", "6 5", "5 4", "4 3" }),
                l1 => Assert.Equal("10", l1),
                l2 => Assert.Equal("10", l2),
                l3 => Assert.Equal("10", l3),
                l4 => Assert.Equal("11", l4));
        }

        [Fact]
        public void FromFile()
        {
            var testfiles = Directory.EnumerateFiles("../../../tests3");
            foreach (var tf in testfiles)
            {
                if (!tf.Contains(".a"))
                {
                    var lines = File.ReadAllLines(tf);
                    var answer = File.ReadAllLines($"{tf}.a");
                    Assert.Equal(answer, MergingTables.Solve(lines[0], lines[1], lines.Skip(2).ToArray()));
                }
            }
        }
    }
}