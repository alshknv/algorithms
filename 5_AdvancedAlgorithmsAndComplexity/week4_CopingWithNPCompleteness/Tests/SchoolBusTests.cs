using _3_school_bus;
using Xunit;

namespace Tests
{
    public class SchoolBusTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Collection(SchoolBus.Solve(new string[] {
                "4 6",
                "1 2 20",
                "1 3 42",
                "1 4 35",
                "2 3 30",
                "2 4 34",
                "3 4 12"
            }),
            l1 => Assert.Equal("97", l1),
            l2 => Assert.Equal("1 4 3 2", l2));
        }

        [Fact]
        public void Example2()
        {
            Assert.Collection(SchoolBus.Solve(new string[] {
                "4 6",
                "1 2 1",
                "2 3 5",
                "1 3 1",
                "1 4 10",
                "2 4 2",
                "3 4 3"
            }),
            l1 => Assert.Equal("7", l1),
            l2 => Assert.Equal("1 3 4 2", l2));
        }

        [Fact]
        public void Triangle()
        {
            Assert.Collection(SchoolBus.Solve(new string[] {
                "3 3",
                "1 2 1",
                "2 3 1",
                "1 3 1"
            }),
            l1 => Assert.Equal("3", l1),
            l2 => Assert.Equal("1 3 2", l2));
        }

        [Fact]
        public void TwoVertices()
        {
            Assert.Collection(SchoolBus.Solve(new string[] {
                "2 1",
                "1 2 1"
            }),
            l1 => Assert.Equal("2", l1),
            l2 => Assert.Equal("1 2", l2));
        }

        [Fact]
        public void NoPath()
        {
            Assert.Collection(SchoolBus.Solve(new string[] {
                "4 4",
                "1 2 1",
                "2 3 4",
                "3 4 5",
                "4 2 1"
            }),
            l1 => Assert.Equal("-1", l1));
        }

        [Fact]
        public void ExtraTest1()
        {
            Assert.Collection(SchoolBus.Solve(new string[] {
                "4 4",
                "1 3 6",
                "1 4 2",
                "2 3 6",
                "2 4 2"
            }),
            l1 => Assert.Equal("16", l1),
            l2 => Assert.Equal("1 4 2 3", l2));
        }

        [Fact]
        public void ExtraTest2()
        {
            Assert.Collection(SchoolBus.Solve(new string[] {
                "4 4",
                "1 2 1",
                "1 4 2",
                "2 3 2",
                "3 4 6"
            }),
            l1 => Assert.Equal("11", l1),
            l2 => Assert.Equal("1 4 3 2", l2));
        }

        [Fact]
        public void ExtraTest3()
        {
            Assert.Collection(SchoolBus.Solve(new string[] {
                "5 6",
                "1 2 3",
                "1 3 1",
                "1 5 5",
                "2 5 9",
                "3 4 8",
                "4 5 5"
            }),
            l1 => Assert.Equal("26", l1),
            l2 => Assert.Equal("1 3 4 5 2", l2));
        }

        [Fact]
        public void ExtraTest4()
        {
            Assert.Collection(SchoolBus.Solve(new string[] {
                "5 7",
                "1 2 1",
                "1 4 9",
                "2 4 1",
                "2 5 9",
                "3 4 6",
                "3 5 8",
                "4 5 1"
            }),
            l1 => Assert.Equal("33", l1),
            l2 => Assert.Equal("1 4 3 5 2", l2));
        }

        [Fact]
        public void Line()
        {
            Assert.Collection(SchoolBus.Solve(new string[] {
                "3 2",
                "1 2 1",
                "2 3 2"
            }),
            l1 => Assert.Equal("-1", l1));
        }

        [Fact]
        public void Hexagon()
        {
            Assert.Collection(SchoolBus.Solve(new string[] {
                "6 15",
                "1 2 12",
                "1 3 29",
                "1 4 22",
                "1 5 13",
                "1 6 24",
                "2 3 19",
                "2 4 3",
                "2 5 25",
                "2 6 6",
                "3 4 21",
                "3 5 23",
                "3 6 28",
                "4 5 4",
                "4 6 5",
                "5 6 16"
            }),
            l1 => Assert.Equal("76", l1),
            l2 => Assert.Equal("1 5 4 6 2 3", l2));
        }
    }
}