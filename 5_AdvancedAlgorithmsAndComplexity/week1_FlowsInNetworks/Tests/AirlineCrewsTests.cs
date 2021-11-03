using System.Linq;
using System.IO;
using System;
using Xunit;
using _2_airline_crews;

namespace Tests
{
    public class AirlineCrewsTests
    {
        [Fact]
        public void AllFileTests()
        {
            var files = Directory.EnumerateFiles("../../../tests2").Where(f => !f.Contains(".a")).ToArray();
            Array.Sort(files);
            foreach (var file in files)
            {
                var input = File.ReadAllLines(file);
                var answerLines = File.ReadAllLines($"{file}.a");
                var answer = answerLines.Length > 0 ? answerLines[0] : "";
                var answerMatchCount = answer.Split(' ').Distinct().Count(x => x != "-1");
                var result = AirlineCrew.Solve(input);
                var resultMatchCount = answer.Split(' ').Distinct().Count(x => x != "-1");
                Assert.Equal(answerMatchCount, resultMatchCount);
            }
        }
    }
}