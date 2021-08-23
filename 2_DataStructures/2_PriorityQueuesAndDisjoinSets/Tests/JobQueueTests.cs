using System.Net;
using System.IO;
using Xunit;
using _2_job_queue;

namespace Tests
{
    public class JobQueueTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Collection(JobQueue.Solve("2 5", "1 2 3 4 5"),
            l1 => Assert.Equal("0 0", l1),
            l2 => Assert.Equal("1 0", l2),
            l3 => Assert.Equal("0 1", l3),
            l4 => Assert.Equal("1 2", l4),
            l5 => Assert.Equal("0 4", l5));
        }

        [Fact]
        public void Example2()
        {
            Assert.Collection(JobQueue.Solve("4 20", "1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1"),
            l1 => Assert.Equal("0 0", l1),
            l2 => Assert.Equal("1 0", l2),
            l3 => Assert.Equal("2 0", l3),
            l4 => Assert.Equal("3 0", l4),
            l5 => Assert.Equal("0 1", l5),
            l6 => Assert.Equal("1 1", l6),
            l7 => Assert.Equal("2 1", l7),
            l8 => Assert.Equal("3 1", l8),
            l9 => Assert.Equal("0 2", l9),
            l10 => Assert.Equal("1 2", l10),
            l11 => Assert.Equal("2 2", l11),
            l12 => Assert.Equal("3 2", l12),
            l13 => Assert.Equal("0 3", l13),
            l14 => Assert.Equal("1 3", l14),
            l15 => Assert.Equal("2 3", l15),
            l16 => Assert.Equal("3 3", l16),
            l17 => Assert.Equal("0 4", l17),
            l18 => Assert.Equal("1 4", l18),
            l19 => Assert.Equal("2 4", l19),
            l20 => Assert.Equal("3 4", l20));
        }

        [Fact]
        public void FromFile()
        {
            var testfiles = Directory.EnumerateFiles("../../../tests2");
            foreach (var tf in testfiles)
            {
                if (!tf.Contains(".a"))
                {
                    var lines = File.ReadAllLines(tf);
                    var answer = File.ReadAllLines($"{tf}.a");
                    Assert.Equal(answer, JobQueue.Solve(lines[0], lines[1]));
                }
            }
        }
    }
}