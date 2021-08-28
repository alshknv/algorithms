using Xunit;
using _6_matching_with_mismatches;

namespace Tests
{
    public class MatchingWithMismatchesTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Collection(MatchingWithMismatches.Solve(new string[]{
                "0 ababab baaa",
                "1 ababab baaa",
                "1 xabcabc ccc",
                "2 xabcabc ccc",
                "3 aaa xxx"
            }),
            l1 => Assert.Equal("0", l1),
            l2 => Assert.Equal("1 1", l2),
            l3 => Assert.Equal("0", l3),
            l4 => Assert.Equal("4 1 2 3 4", l4),
            l5 => Assert.Equal("1 0", l5));
        }

        [Fact]
        public void Example2()
        {
            Assert.Collection(MatchingWithMismatches.Solve(new string[] { "2 baababaabaaaa aababaaa" }),
                l1 => Assert.Equal("2 1 4", l1));
        }
    }
}