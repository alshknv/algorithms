using System.Diagnostics;
using System.IO;
using System;
using Xunit;
using _1_gsm_network;

namespace Tests
{
    public class GsmNetworkTests
    {
        private string[] Minisat(string[] result)
        {
            var firstline = result[0].Split(' ');
            result[0] = $"p cnf {firstline[1]} {firstline[0]}";
            var fname = Guid.NewGuid().ToString("N");
            File.WriteAllLines($"in{fname}", result);
            var args = $"in{fname} out{fname}";
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "minisat",
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            process.Start();
            process.WaitForExit();
            var satResult = File.ReadAllLines($"out{fname}");
            File.Delete($"in{fname}");
            File.Delete($"out{fname}");
            return satResult;
        }

        [Fact]
        public void SAT()
        {
            var input = new string[] { "3 3", "1 2", "2 3", "1 3" };
            var satResult = Minisat(GsmNetwork.Solve(input));
            Assert.Equal("SAT", satResult[0]);
        }

        [Fact]
        public void UNSAT()
        {
            var input = new string[] { "4 6", "1 2", "1 3", "1 4", "2 3", "2 4", "3 4" };
            var satResult = Minisat(GsmNetwork.Solve(input));
            Assert.Equal("UNSAT", satResult[0]);
        }
    }
}
