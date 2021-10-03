using _1_friend_suggestion;
using Xunit;

namespace Tests
{
    public class FriendSuggestionTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Collection(FriendSuggestion.Solve(4, new string[] { "1 2 1", "4 1 2", "2 3 2", "1 3 5" }, new string[] { "1 3" }),
                l1=>Assert.Equal("3",l1));
        }
    }
}
