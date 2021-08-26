using Xunit;
using _4_substring_equality;

namespace Tests
{
    public class SubstringEqualityTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Collection(SubstringEquality.Solve("trololo", new string[] { "0 0 7", "2 4 3", "3 5 1", "1 3 2" }),
            l1 => Assert.Equal("Yes", l1),
            l2 => Assert.Equal("Yes", l2),
            l3 => Assert.Equal("Yes", l3),
            l4 => Assert.Equal("No", l4));
        }

        [Fact]
        public void Stress()
        {
            for (int i = 0; i < 100000; i++)
            {
                Assert.Collection(SubstringEquality.Solve("trololotrololotrololo", new string[] { "0 0 21", "2 4 3", "3 5 1", "1 3 2" }),
                    l1 => Assert.Equal("Yes", l1),
                    l2 => Assert.Equal("Yes", l2),
                    l3 => Assert.Equal("Yes", l3),
                    l4 => Assert.Equal("No", l4));
            }
        }
    }
}