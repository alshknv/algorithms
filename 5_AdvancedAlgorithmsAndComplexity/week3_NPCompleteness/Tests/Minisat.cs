using System.IO;
using System;
using System.Diagnostics;

namespace Tests
{
    public static class Minisat
    {
        private static string[] Result(string[] result)
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

        public static bool SAT(string[] clauses)
        {
            var satResult = Result(clauses);
            return satResult[0] == "SAT";
        }

        public static bool UNSAT(string[] clauses)
        {
            var satResult = Result(clauses);
            return satResult[0] == "UNSAT";
        }
    }
}