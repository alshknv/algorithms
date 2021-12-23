using System;
using Xunit;
using _1_puzzle_assembly;

namespace Tests
{
    public class PuzzleAssemblyTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Collection(PuzzleAssembly.Solve(new string[]{"(yellow,black,black,blue)",
                                        "(blue,blue,black,yellow)",
                                        "(orange,yellow,black,black)",
                                        "(red,black,yellow,green)",
                                        "(orange,green,blue,blue)",
                                        "(green,blue,orange,black)",
                                        "(black,black,red,red)",
                                        "(black,red,orange,purple)",
                                        "(black,purple,green,black)"}),
                                        l1 => Assert.Equal("(black,black,red,red);(black,red,orange,purple);(black,purple,green,black)", l1),
                                        l2 => Assert.Equal("(red,black,yellow,green);(orange,green,blue,blue);(green,blue,orange,black)", l2),
                                        l3 => Assert.Equal("(yellow,black,black,blue);(blue,blue,black,yellow);(orange,yellow,black,black)", l3));
        }
    }
}
