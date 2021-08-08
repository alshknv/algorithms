using System.Diagnostics;
using System.Linq;
using System.Text;
using System;
using Xunit;
using _5_organizing_a_lottery;
using System.Collections.Generic;

namespace Tests
{
    public class OrganisingALotteryTests
    {
        [Fact]
        public void Test1()
        {
            var result = OrganizingLottery.Solve(
                "1 6 11", new string[] {
                    "0 5",
                    "7 10"
            });
            Assert.Equal("1 0 0", result);
        }

        [Fact]
        public void Test2()
        {
            var result = OrganizingLottery.Solve(
                "-100 100 0", new string[] {
                    "-10 10"
            });
            Assert.Equal("0 0 1", result);
        }

        [Fact]
        public void Test3()
        {
            var result = OrganizingLottery.Solve(
                "1 6", new string[] {
                    "0 5",
                    "-3 2",
                    "7 10"
            });
            Assert.Equal("2 0", result);
        }

        [Fact]
        public void Test4()
        {
            var result = OrganizingLottery.Solve(
                "-1 8 1 6", new string[] {
                    "0 5",
                    "-3 2",
                    "7 10"
            });
            Assert.Equal("1 1 2 0", result);
        }

        [Fact]
        public void Stress()
        {
            var points = new int[50000];
            var lines = new string[50000];
            for (int i = 0; i < 50000; i++)
            {
                points[i] = i;
                lines[i] = $"{i} {i + 2}";
            }
            var target = new List<int>() { 1, 2 };
            target.AddRange(Enumerable.Repeat(3, 49998));

            Stopwatch watch = new Stopwatch();
            watch.Start();
            var result = OrganizingLottery.Solve(string.Join(" ", points), lines);
            watch.Stop();
            Assert.Equal(string.Join(" ", target), result);
            Assert.True(watch.ElapsedMilliseconds < 500);
        }
    }
}
