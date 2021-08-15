using Xunit;
using _5_longest_common_subsequence_of_three_sequences;

namespace Tests
{
    public class LongestCommonSubsequenceOfThreeTests
    {
        [Fact]
        public void Simple()
        {
            Assert.Equal("2", CommonSubsequence3.Solve("1 2 3", "2 1 3", "1 3 5"));
        }

        [Fact]
        public void Longer()
        {
            Assert.Equal("3", CommonSubsequence3.Solve("8 3 2 1 7", "8 2 1 3 8 10 7", "6 8 3 1 4 7"));
        }

        [Fact]
        public void TwoShortAndLong()
        {
            Assert.Equal("1", CommonSubsequence3.Solve("8", "1 8", "6 8 3 1 4 7"));
        }

        [Fact]
        public void ShortAndLong()
        {
            Assert.Equal("1", CommonSubsequence3.Solve("8", "1 8", "1 6 8 3 1 4 7"));
        }

        [Fact]
        public void Tricky()
        {
            Assert.Equal("0", CommonSubsequence3.Solve("1 2 3", "3 2 0", "0 1"));
        }
    }
}