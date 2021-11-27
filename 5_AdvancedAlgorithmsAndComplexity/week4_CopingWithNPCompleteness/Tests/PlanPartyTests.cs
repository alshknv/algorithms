using _2_plan_party;
using Xunit;

namespace Tests
{
    public class PlanPartyTests
    {
        [Fact]
        public void OnlyCEO()
        {
            Assert.Equal("1000", PlanParty.Solve(new string[] {
                "1",
                "1000"
            }));
        }

        [Fact]
        public void TwoPeople()
        {
            Assert.Equal("2", PlanParty.Solve(new string[] {
                "2",
                "1 2",
                "1 2"
            }));
        }

        [Fact]
        public void Company()
        {
            Assert.Equal("11", PlanParty.Solve(new string[] {
                "5",
                "1 5 3 7 5",
                "5 4",
                "2 3",
                "4 2",
                "1 2"
            }));
        }
    }
}