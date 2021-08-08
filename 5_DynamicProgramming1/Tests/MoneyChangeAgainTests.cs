using Xunit;
using _1_money_change_again;

namespace Tests
{
    public class MoneyChangeAgainTests
    {
        [Fact]
        public void Simple()
        {
            Assert.Equal("2", MoneyChangeAgain.Solve("2"));
            Assert.Equal("2", MoneyChangeAgain.Solve("6"));
            Assert.Equal("9", MoneyChangeAgain.Solve("34"));
        }

        [Fact]
        public void NaiveStress()
        {
            for (int m = 1; m <= 100; m++)
            {
                Assert.Equal(MoneyChangeAgain.Naive(m.ToString()), MoneyChangeAgain.Solve(m.ToString()));
            }
        }
    }
}