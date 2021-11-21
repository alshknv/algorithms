using System;
using Xunit;
using _3_budget_allocation;

namespace Tests
{
    public class BudgetAllocationTests
    {
        [Fact]
        public void SAT1()
        {
            var input = new string[] { "2 3", "5 2 3", "-1 -1 -1", "6 -2" };
            Assert.True(Minisat.SAT(BudgetAllocation.Solve(input)));
        }

        [Fact]
        public void SAT2()
        {
            var input = new string[] { "3 3", "1 0 0", "0 1 0", "0 0 1", "1 1 1" };
            Assert.True(Minisat.SAT(BudgetAllocation.Solve(input)));
        }

        [Fact]
        public void UNSAT1()
        {
            var input = new string[] { "2 1", "1", "-1", "0 -1" };
            Assert.True(Minisat.UNSAT(BudgetAllocation.Solve(input)));
        }

        [Fact]
        public void UNSAT2()
        {
            var input = new string[] { "2 3", "1 1 0", "0 -1 -1", "0 -2" };
            Assert.True(Minisat.UNSAT(BudgetAllocation.Solve(input)));
        }

        [Fact]
        public void UNSAT3()
        {
            var input = new string[] { "1 1", "5", "-1" };
            Assert.True(Minisat.UNSAT(BudgetAllocation.Solve(input)));
        }

        [Fact]
        public void ZeroUNSAT()
        {
            var input = new string[] { "3 3", "5 2 3", "0 0 1", "0 0 0", "6 -1 0" };
            Assert.True(Minisat.UNSAT(BudgetAllocation.Solve(input)));
        }

        [Fact]
        public void ZeroSAT()
        {
            var input = new string[] { "2 5", "0 0 5 2 3", "1 0 0 0 0", "6 3" };
            Assert.True(Minisat.SAT(BudgetAllocation.Solve(input)));
        }
    }
}
