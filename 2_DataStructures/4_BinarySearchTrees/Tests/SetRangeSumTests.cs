using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;
using _4_set_range_sum;

namespace Tests
{
    public class SetRangeSumTests
    {
        [Fact]
        public void Examples()
        {
            Assert.Collection(SetRangeSum.Solve(new string[] {
                "? 1",
                "+ 1",
                "? 1",
                "+ 2",
                "s 1 2",
                "+ 1000000000",
                "? 1000000000",
                "- 1000000000",
                "? 1000000000",
                "s 999999999 1000000000",
                "- 2",
                "? 2",
                "- 0",
                "+ 9",
                "s 0 9"
            }),
            l1 => Assert.Equal("Not found", l1),
            l2 => Assert.Equal("Found", l2),
            l3 => Assert.Equal("3", l3),
            l4 => Assert.Equal("Found", l4),
            l5 => Assert.Equal("Not found", l5),
            l6 => Assert.Equal("1", l6),
            l7 => Assert.Equal("Not found", l7),
            l8 => Assert.Equal("10", l8));

            Assert.Collection(SetRangeSum.Solve(new string[] {
                "? 0",
                "+ 0",
                "? 0",
                "- 0",
                "? 0"
            }),
            l1 => Assert.Equal("Not found", l1),
            l2 => Assert.Equal("Found", l2),
            l3 => Assert.Equal("Not found", l3));

            Assert.Collection(SetRangeSum.Solve(new string[] {
                "+ 491572259",
                "? 491572259",
                "? 899375874",
                "s 310971296 877523306",
                "+ 352411209"
            }),
            l1 => Assert.Equal("Found", l1),
            l2 => Assert.Equal("Not found", l2),
            l3 => Assert.Equal("491572259", l3));
        }

        [Fact]
        public void FromFile()
        {
            var testfiles = Directory.EnumerateFiles("../../../tests4");
            foreach (var tf in testfiles)
            {
                if (!tf.Contains(".a"))
                {
                    Stopwatch w = new Stopwatch();
                    var lines = File.ReadAllLines(tf);
                    var answer = File.ReadAllLines($"{tf}.a");
                    w.Start();
                    Assert.Equal(answer, SetRangeSum.Solve(lines.Skip(1).ToArray()));
                    w.Stop();
                    Assert.True(w.ElapsedMilliseconds < 1500);
                }
            }
        }
    }
}