using System.Linq;
using System.IO;
using System;
using Xunit;
using _3_stock_charts;

namespace Tests
{
    public class StockChartsTests
    {
        [Fact]
        public void AllFileTests()
        {
            var files = Directory.EnumerateFiles("../../../tests3").Where(f => !f.Contains(".a"));
            foreach (var file in files)
            {
                var input = File.ReadAllLines(file);
                var answer = File.ReadAllLines($"{file}.a");
                Assert.Equal(answer.Length > 0 ? answer[0] : "", StockCharts.Solve(input));
            }
        }
    }
}