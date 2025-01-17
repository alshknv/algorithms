using System;
using Xunit;
using _5_longest_common_substring;

namespace Tests
{
    public class LongestCommonSubstringTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Collection(LongestCommonSubstring.Solve(new string[] {
                "cool toolbox",
                "aaa bb",
                "aabaa babbaab"
            }),
            l1 => Assert.Equal("1 1 3", l1),
            l2 => Assert.Equal("0 0 0", l2),
            l3 => Assert.Equal("0 4 3", l3));
        }

        [Fact]
        public void Example2()
        {
            Assert.Collection(LongestCommonSubstring.Solve(new string[] {
                "baababbbbabaababaa babbabaababaabbaabaa",
                "baaababbbbaa aababaabaaaabbaabaa",
                "bbababbbbbbaaabbbb bbbababbbaaaaaa",
                "aabaabbbabbaaaaba aabaababaaababababab",
                "babababaaaaababaab bbbabbaababb",
                "babbbabbabb abbaabaaab",
                "abaaabbbbbababbaabb bbbbbaaabbb",
                "baaaaaababbbbbbaaaa aababbbbbabaaaab",
                "abaaabaabbaa aaabbbaaaabb",
                "bbaabbabaaabbbbb aaabbbbbbbbb",
                "bbbabbbababbba baaabbaaabb",
                "baaaaaaaab bbababbbaba",
                "aaabaaabaabbaba bbbaabbbabbabbab"
            }),
            l1 => Assert.Equal("7 2 11", l1),
            l2 => Assert.Equal("2 0 5", l2),
            l3 => Assert.Equal("0 1 8", l3),
            l4 => Assert.Equal("0 0 6", l4),
            l5 => Assert.Equal("10 6 5", l5),
            l6 => Assert.Equal("5 0 4", l6),
            l7 => Assert.Equal("1 4 7", l7),
            l8 => Assert.Equal("5 0 9", l8),
            l9 => Assert.Equal("1 5 4", l9),
            l10 => Assert.Equal("8 0 8", l10),
            l11 => Assert.Equal("9 3 3", l11),
            l12 => Assert.Equal("0 9 2", l12),
            l13 => Assert.Equal("7 2 5", l13));
        }

        [Fact]
        public void Stress()
        {
            var rnd = new Random();
            var len1 = rnd.Next(50) + 15;
            var len2 = rnd.Next(50) + 15;
            var count = 10;
            var repetitions = 1000;
            for (int r = 0; r < repetitions; r++)
            {
                var input = new string[count];
                for (int c = 0; c < count; c++)
                {
                    var str = "";
                    for (int i = 0; i < len1; i++)
                    {
                        str += rnd.Next(100) % 2 == 0 ? "a" : "b";
                    }
                    str += " ";
                    for (int i = 0; i < len2; i++)
                    {
                        str += rnd.Next(100) % 2 == 0 ? "a" : "b";
                    }
                    input[c] = str;
                }
                LongestCommonSubstring.Solve(input);
            }
        }

        [Fact]
        public void Zero()
        {
            Assert.Collection(LongestCommonSubstring.Solve(new string[] {
                "aaa ", " "
            }),
            l1 => Assert.Equal("0 0 0", l1),
            l2 => Assert.Equal("0 0 0", l2));
        }
    }
}