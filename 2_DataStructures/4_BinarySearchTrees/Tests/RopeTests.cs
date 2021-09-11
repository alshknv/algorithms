using System;
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

        [Fact]
        public void NaiveStress()
        {
            var count = 100;
            for (int j = 0; j < 100; j++)
            {
                string input = "";
                Random random = new Random();
                for (int i = 0; i < count; i++)
                {
                    input += Convert.ToChar((int)Math.Floor(26 * random.NextDouble() + 65));
                }
                var queries = new string[count / 10];
                for (int i = 0; i < count / 10; i++)
                {
                    var x1 = random.Next(count - 1);
                    var x2 = random.Next(count - x1) + x1;
                    var x3 = random.Next(count - (x2 - x1) - 1);
                    queries[i] = $"{x1} {x2} {x3}";
                }
                Assert.Equal(Rope.Naive(input, queries), Rope.Solve(input, queries));
            }
        }
    }
}