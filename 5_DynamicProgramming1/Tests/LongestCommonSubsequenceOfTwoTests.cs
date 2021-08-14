using Xunit;
using _4_longest_common_subsequence_of_two_sequences;

namespace Tests
{
    public class LongestCommonSubsequenceOfTwoTests
    {
        [Fact]
        public void Simple()
        {
            Assert.Equal("2", CommonSubsequence2.Solve("2 7 5", "2 5"));
        }

        [Fact]
        public void OneNumber()
        {
            Assert.Equal("1", CommonSubsequence2.Solve("1", "1"));
            Assert.Equal("0", CommonSubsequence2.Solve("1", "5"));
        }

        [Fact]
        public void NoCommon()
        {
            Assert.Equal("0", CommonSubsequence2.Solve("7", "1 2 3 4"));
            Assert.Equal("0", CommonSubsequence2.Solve("1 3 5 7 9 11", "0 2 4 6 8 10 12"));
        }

        [Fact]
        public void FewNumbers()
        {
            Assert.Equal("2", CommonSubsequence2.Solve("1 2 3", "2 1 3"));
        }

        [Fact]
        public void Longer()
        {
            Assert.Equal("4", CommonSubsequence2.Solve("1 5 9 3 4 6 2 7 8 3 5", "1 0 0 3 0 0 8 0 0 5"));
        }

        [Fact]
        public void EqualNumbers()
        {
            Assert.Equal("1", CommonSubsequence2.Solve("0 0 0", "0"));
            Assert.Equal("3", CommonSubsequence2.Solve("0 0 0", "0 0 0 0 0"));
        }
    }
}