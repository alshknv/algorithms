using System;
using System.Collections.Generic;
using Xunit;
using _5_collecting_signatures;

namespace Tests
{
    public class CollectingSignaturesTests
    {
        [Fact]
        public void Test1()
        {
            Assert.Collection(CollectingSignatures.Solve(new List<string>() {
                "1 3",
                "2 5",
                "3 6"
            }),
            l1 => Assert.Equal("1", l1),
            l2 => Assert.Equal("3", l2));
        }

        [Fact]
        public void Test2()
        {
            Assert.Collection(CollectingSignatures.Solve(new List<string>() {
                "4 7",
                "1 3",
                "2 5",
                "5 6"
            }),
            l1 => Assert.Equal("2", l1),
            l2 => Assert.Equal("3 6", l2));
        }

        [Fact]
        public void Test3()
        {
            Assert.Collection(CollectingSignatures.Solve(new List<string>() {
                "400000000 700000000",
                "100000000 300000000",
                "200000000 500000000",
                "500000000 600000000"
            }),
            l1 => Assert.Equal("2", l1),
            l2 => Assert.Equal("300000000 600000000", l2));
        }

        [Fact]
        public void Test4()
        {
            Assert.Collection(CollectingSignatures.Solve(new List<string>() {
                "2 7",
                "4 8",
                "1 5",
                "3 6"
            }),
            l1 => Assert.Equal("1", l1),
            l2 => Assert.Equal("5", l2));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal("43", CollectingSignatures.Solve(new List<string>(){
                "41 42", "52 52","63 63","80 82","78 79","35 35","22 23","31 32","44 45","81 82","36 38","10 12","1 1","23 23","32 33",
                "87 88","55 56","69 71","89 91","93 93","38 40","33 34","14 16","57 59","70 72","36 36","29 29","73 74","66 68","36 38",
                "1 3",
                "49 50",
                "68 70",
                "26 28",
                "30 30",
                "1 2",
                "64 65",
                "57 58",
                "58 58",
                "51 53",
                "41 41",
                "17 18",
                "45 46",
                "4 4",
                "0 1",
                "65 67",
                "92 93",
                "84 85",
                "75 77",
                "39 41",
                "15 15",
                "29 31",
                "83 84",
                "12 14",
                "91 93",
                "83 84",
                "81 81",
                "3 4",
                "66 67",
                "8 8",
                "17 19",
                "86 87",
                "44 44",
                "34 34",
                "74 74",
                "94 95",
                "79 81",
                "29 29",
                "60 61",
                "58 59",
                "62 62",
                "54 56",
                "58 58",
                "79 79",
                "89 91",
                "40 42",
                "2 4",
                "12 14",
                "5 5",
                "28 28",
                "35 36",
                "7 8",
                "82 84",
                "49 51",
                "2 4",
                "57 59",
                "25 27",
                "52 53",
                "48 49",
                "9 9",
                "10 10",
                "78 78",
                "26 26",
                "83 84",
                "22 24",
                "86 87",
                "52 54",
                "49 51",
                "63 64",
                "54 54"
            })[0]);
        }
    }
}
