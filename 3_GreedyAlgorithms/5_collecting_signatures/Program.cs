using System.Collections.Generic;
using System;

namespace _5_collecting_signatures
{
    public class CollectingSignatures
    {
        public static List<string> Solve(List<string> input)
        {
            return new List<string>();
        }

        static void Main(string[] args)
        {
            var lines = new List<string>();
            for (int i = 0; i < int.Parse(lines[0]); i++)
            {
                lines.Add(Console.ReadLine());
            }
            foreach (var line in Solve(lines))
                Console.WriteLine(line);
        }
    }
}
