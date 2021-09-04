using System.IO;
using System.Linq;
using Xunit;
using _2_is_bst;

namespace Tests
{
    public class IsBstTests
    {
        [Fact]
        public void Correct()
        {
            Assert.Equal("CORRECT", IsBst.Solve(new string[] { }));
            Assert.Equal("CORRECT", IsBst.Solve(new string[] {
                "2 1 2",
                "1 -1 -1",
                "3 -1 -1"
            }));
            Assert.Equal("CORRECT", IsBst.Solve(new string[] {
                "1 -1 1",
                "2 -1 2",
                "3 -1 3",
                "4 -1 4",
                "5 -1 -1"
            }));
            Assert.Equal("CORRECT", IsBst.Solve(new string[] {
                "4 1 2",
                "2 3 4",
                "6 5 6",
                "1 -1 -1",
                "3 -1 -1",
                "5 -1 -1",
                "7 -1 -1"
            }));
        }

        [Fact]
        public void Incorrect()
        {
            Assert.Equal("INCORRECT", IsBst.Solve(new string[] {
                "1 1 2",
                "2 -1 -1",
                "3 -1 -1"
            }));
            Assert.Equal("INCORRECT", IsBst.Solve(new string[] {
                "4 1 -1",
                "2 2 3",
                "1 -1 -1",
                "5 -1 -1"
            }));
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
                    var answer = IsBst.Solve(lines.Skip(1).ToArray());
                    Assert.True(answer == "CORRECT" || answer == "INCORRECT");
                }
            }
        }
    }
}