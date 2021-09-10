using Xunit;
using _5_rope;

namespace Tests
{
    public class RopeTests
    {
        [Fact]
        public void Examples()
        {
            Assert.Equal("helloworld", Rope.Solve("hlelowrold", new string[] {
                "1 1 2",
                "6 6 7"
            }));
            Assert.Equal("efcabd", Rope.Solve("abcdef", new string[] {
                "0 1 1",
                "4 5 0"
            }));
        }
    }
}