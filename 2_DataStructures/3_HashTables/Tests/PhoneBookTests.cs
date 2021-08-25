using System;
using Xunit;
using _1_phone_book;

namespace Tests
{
    public class PhoneBookTests
    {
        [Fact]
        public void Example1()
        {
            var queries = new string[] {
                "add 911 police",
                "add 76213 Mom",
                "add 17239 Bob",
                "find 76213",
                "find 910",
                "find 911",
                "del 910",
                "del 911",
                "find 911",
                "find 76213",
                "add 76213 daddy",
                "find 76213"
            };
            Assert.Collection(PhoneBook.Solve(queries),
                l1 => Assert.Equal("Mom", l1),
                l2 => Assert.Equal("not found", l2),
                l3 => Assert.Equal("police", l3),
                l4 => Assert.Equal("not found", l4),
                l5 => Assert.Equal("Mom", l5),
                l6 => Assert.Equal("daddy", l6));
        }

        [Fact]
        public void Example2()
        {
            var queries = new string[] {
                "find 3839442",
                "add 123456 me",
                "add 0 granny",
                "find 0",
                "find 123456",
                "del 0",
                "del 0",
                "find 0"
            };
            Assert.Collection(PhoneBook.Solve(queries),
                l1 => Assert.Equal("not found", l1),
                l2 => Assert.Equal("granny", l2),
                l3 => Assert.Equal("me", l3),
                l4 => Assert.Equal("not found", l4));
        }

        [Fact]
        public void MiniStress()
        {
            for (int i = 0; i < 1000; i++)
            {
                Example1();
                Example2();
            }
        }
    }
}
