using System;
using Xunit;
using _2_cleaning_apartment;

namespace Tests
{
    public class CleaningApartmentTests
    {
        [Fact]
        public void SAT()
        {
            var input = new string[] { "5 4", "1 2", "2 3", "3 5", "4 5" };
            Assert.True(Minisat.SAT(CleaningApartment.Solve(input)));
        }

        [Fact]
        public void UNSAT()
        {
            var input = new string[] { "4 3", "1 2", "1 3", "1 4" };
            Assert.True(Minisat.UNSAT(CleaningApartment.Solve(input)));
        }

        [Fact]
        public void SAT2()
        {
            var input = new string[] { "3 3", "1 2", "1 3" };
            Assert.True(Minisat.SAT(CleaningApartment.Solve(input)));
        }
    }
}
