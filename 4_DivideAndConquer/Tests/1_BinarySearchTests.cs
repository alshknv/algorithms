using System.Collections.Generic;
using System;
using System.Diagnostics;
using Xunit;
using _1_binary_search;

namespace Tests
{
    public class BinarySearchTests
    {
        [Fact]
        public void Test1()
        {
            var result = BinarySearch.Solve(
                "5 1 5 8 12 13",
                "5 8 1 23 1 11");
            Assert.Equal("2 0 -1 0 -1", result);
        }

        [Fact]
        public void Test2()
        {
            var result = BinarySearch.Solve(
                "5 1 5 8 12 13",
                "25 8 1 23 1 11 8 1 23 1 11 8 1 23 1 11 8 1 23 1 11 8 1 23 1 11");
            Assert.Equal("2 0 -1 0 -1 2 0 -1 0 -1 2 0 -1 0 -1 2 0 -1 0 -1 2 0 -1 0 -1", result);
        }

        [Fact]
        public void Stress()
        {
            var array = new List<int>();
            var search = new List<int>();
            for (int i = 0; i < 1000000; i++)
            {
                if (i < 10000) array.Add(i);
                search.Add(i % 10000);
            }
            var arrayString = $"{array.Count} {string.Join(" ", array)}";
            var searchString = $"{search.Count} {string.Join(" ", search)}";
            var resultString = $"{string.Join(" ", search)}";
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = BinarySearch.Solve(arrayString, searchString);
            stopwatch.Stop();
            Assert.Equal(resultString, result);
            Assert.True(stopwatch.ElapsedMilliseconds < 1400);
        }
    }
}
