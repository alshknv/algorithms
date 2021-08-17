using Xunit;
using _2_tree_height;

namespace Tests
{
    public class TreeHeightTests
    {
        [Fact]
        public void Examples()
        {
            Assert.Equal("3", TreeHeight.Solve("4 -1 4 1 1"));
            Assert.Equal("4", TreeHeight.Solve("-1 0 4 0 3"));
        }

        [Fact]
        public void Complex()
        {
            Assert.Equal("70", TreeHeight.Solve("59 30 0 1 80 51 83 25 40 35 59 77 22 31 47 41 56 36 68 9 89 6 72 44 5 54 23 63 70 87 90 22 8 18 71 92 97 29 48 97 14 62 55 89 15 15 82 55 88 2 19 36 0 91 64 81 69 11 60 56 34 77 98 70 96 20 50 57 8 4 13 93 65 57 53 3 20 5 64 3 21 -1 95 67 30 4 18 34 6 94 46 23 27 53 75 7 58 86 52 14"));
            Assert.Equal("10", TreeHeight.Solve("50 73 65 90 58 60 80 94 -1 38 15 23 42 73 56 8 61 9 30 86 45 30 57 8 45 66 69 80 22 66 53 2 48 22 86 30 44 8 60 98 82 24 91 21 8 65 77 77 62 54 26 47 46 75 29 15 67 23 54 81 28 86 66 18 72 60 8 18 92 48 9 15 76 8 81 57 48 76 7 71 28 23 29 30 57 76 29 89 66 2 30 7 44 27 37 29 57 6 66 36"));
        }
    }
}