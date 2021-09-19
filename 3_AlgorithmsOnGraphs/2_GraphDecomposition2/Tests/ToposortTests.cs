using System.Collections.Generic;
using System;
using Xunit;
using _2_toposort;

namespace Tests
{
    public class ToposortTests
    {
        [Fact]
        public void Example1()
        {
            Assert.Equal("4 3 1 2", Toposort.Solve(new string[]{
                "4 3",
                "1 2",
                "4 1",
                "3 1"
            }));
        }

        [Fact]
        public void Example2()
        {
            Assert.Equal("4 3 2 1", Toposort.Solve(new string[]{
                "4 1",
                "3 1"
            }));
        }

        [Fact]
        public void Example3()
        {
            Assert.Equal("5 4 3 2 1", Toposort.Solve(new string[]{
                "5 7",
                "2 1",
                "3 2",
                "3 1",
                "4 3",
                "4 1",
                "5 2",
                "5 3"
            }));
        }

        [Fact]
        public void Example4()
        {
            Assert.Equal("16 15 8 3 4 7 6 10 11 2 5 9 12 13 14 17 18 1",
            Toposort.Solve(new string[]{
                "18 21",
                "2 5",
                "3 6",
                "3 7",
                "3 4",
                "4 18",
                "4 7",
                "5 9",
                "6 9",
                "6 10",
                "7 11",
                "8 12",
                "9 12",
                "9 13",
                "10 14",
                "10 11",
                "11 18",
                "12 13",
                "13 14",
                "14 17",
                "17 18",
                "16 18",
            }));
        }

        [Fact]
        public void Example5()
        {
            Assert.Equal("6 1 3 2 4 5", Toposort.Solve(new string[]{
                "6 9",
                "1 2",
                "1 3",
                "2 4",
                "3 4",
                "2 5",
                "3 5",
                "4 5",
                "6 3",
                "6 1"
            }));
        }

        [Fact]
        public void Stress()
        {
            var input = new string[100001];
            input[0] = "100000 100000";
            var dict = new Dictionary<(int, int), bool>();
            var rnd = new Random();
            for (int k = 0; k < 10; k++)
            {
                for (int i = 1; i <= 100000; i++)
                {
                    int s, d;
                    do
                    {
                        s = rnd.Next(100000) + 1;
                        d = rnd.Next(100000) + 1;
                    } while (dict.ContainsKey((s, d)));
                    input[i] = $"{s} {d}";
                }
                Toposort.Solve(input);
            }
        }
    }
}