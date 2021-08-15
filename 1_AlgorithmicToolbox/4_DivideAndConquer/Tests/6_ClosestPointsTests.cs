using System.Reflection;
using System.Diagnostics;
using System;
using Xunit;
using Xunit.Abstractions;
using _6_closest_points;

namespace Tests
{
    public class ClosestPointsTests
    {

        [Fact]
        public void Test1()
        {
            Assert.Equal("5", ClosestPoints.Solve(new string[] {
                "0 0",
                "3 4"
            }));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("0", ClosestPoints.Solve(new string[] {
                "7 7",
                "1 100",
                "4 8",
                "7 7"
            }));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal("1.4142", ClosestPoints.Solve(new string[] {
                "4 4",
                "-2 -2",
                "-3 -4",
                "-1 3",
                "2 3",
                "-4 0",
                "1 1",
                "-1 -1",
                "3 -1",
                "-4 2",
                "-2 4"
            }));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal("2.8284", ClosestPoints.Solve(new string[] {
                "7 9",
                "-6 -6",
                "-3 -4",
                "-1 3",
                "2 9",
                "-4 0",
                "1 1",
                "-1 -1",
                "4 -6",
                "-6 4",
                "-3 7",
                "6 2",
                "8 -1",
                "7 -6"
            }));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal("2.0", ClosestPoints.Solve(new string[] {
                "-1 1",
                "1 -1",
                "-1 3",
                "1 1",
                "1 3",
                "-1 -3",
                "1 -3",
                "-1 -1"
            }));
        }

        [Fact]
        public void Test6()
        {
            Assert.Equal("2.8284", ClosestPoints.Solve(new string[] {
                $"{-1000000000} {-1000000000}",
                $"{1000000000} {1000000000}",
                "-1 -1",
                "1 1"
            }));
        }

        [Fact]
        public void Test7()
        {
            Assert.Equal("2.2361", ClosestPoints.Solve(new string[] {
                "-9 -2",
                "-4 -8",
                "-4 -1",
                "-1 7",
                "1 10",
                "1 6"
            }));
        }

        [Fact]
        public void Stress()
        {
            Stopwatch watch = new Stopwatch();
            var rnd = new Random();
            var results = new string[50000];
            var points = new string[50000];
            for (int j = 0; j < 50000; j++)
            {
                points[j] = $"{rnd.Next(int.MaxValue)} {rnd.Next(int.MaxValue)}";
            }
            var idx = 0;
            watch.Start();
            results[idx++] = ClosestPoints.Solve(points);
            watch.Stop();
            Assert.Equal(50000, results.Length);
            Assert.True(watch.ElapsedMilliseconds < 3000);
        }

        [Fact]
        public void StressNaive()
        {
            var rnd = new Random();
            for (int i = 0; i < 10000; i++)
            {
                var points = new string[20];
                for (int j = 0; j < 20; j++)
                {
                    points[j] = $"{-rnd.Next(1000000)} {rnd.Next(1000000)}";
                }
                var naiveOutput = ClosestPoints.Naive(points);
                var realOutput = ClosestPoints.Solve(points);
                Assert.Equal(naiveOutput, realOutput);
            }
        }
    }
}
