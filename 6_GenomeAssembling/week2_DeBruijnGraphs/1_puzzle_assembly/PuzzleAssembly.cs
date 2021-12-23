using System;

namespace _1_puzzle_assembly
{
    public static class PuzzleAssembly
    {
        public static string[] Solve(string[] puzzle)
        {
            return Array.Empty<string>();
        }

        static void Main(string[] args)
        {
            var puzzle = new string[25];
            for (int i = 0; i < 25; i++)
                puzzle[i] = Console.ReadLine();
            foreach (var line in Solve(puzzle))
                Console.WriteLine(line);
        }
    }
}
