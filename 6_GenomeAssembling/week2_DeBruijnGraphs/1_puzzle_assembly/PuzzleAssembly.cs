using System.Collections.Generic;
using System;
using System.Linq;

namespace _1_puzzle_assembly
{
    public static class PuzzleAssembly
    {
        private static int n;
        private static string[][] grid;
        private static int[] GetNeighbors(int x)
        {
            return new int[4] {
                x < n ? -1 : x - n,
                x % n == 0 ? -1 : x - 1,
                x + n >= n * n ? -1 : x + n,
                (x + 1) % n == 0 ? -1 : x + 1
            };
        }
        private static bool CanBePlaced(string[] piece, int cell)
        {
            var neighbors = GetNeighbors(cell);
            var neighborEdges = 2;
            for (int e = 0; e < 4; e++)
            {
                //if it is border - black color, if empty cell - any color is allowed, otherwise colors should match
                if ((neighbors[e] < 0 ? "black" : (grid[neighbors[e]] == null ? piece[e] : grid[neighbors[e]][neighborEdges])) != piece[e])
                    return false;
                if (++neighborEdges > 3) neighborEdges = 0;
            }
            return true;
        }

        private static bool FillGrid(List<string[]> pieces, int cell)
        {
            // recursive filling with backtrack
            for (int p = 0; p < pieces.Count; p++)
            {
                if (CanBePlaced(pieces[p], cell))
                {
                    grid[cell] = pieces[p];
                    if (cell == grid.Length - 1)
                        return true;
                    var nextPieces = new List<string[]>(pieces);
                    nextPieces.RemoveAt(p);
                    if (!FillGrid(nextPieces, cell + 1))
                        grid[cell] = null;
                    else return true;
                }
            }
            return false;
        }

        public static string[] Solve(string[] puzzle)
        {
            n = (int)Math.Sqrt(puzzle.Length);
            grid = new string[puzzle.Length][];
            var pieces = new List<string[]>();
            for (int i = 0; i < puzzle.Length; i++)
                pieces.Add(puzzle[i].Substring(1, puzzle[i].Length - 2).Split(','));
            if (!FillGrid(pieces, 0)) return new string[] { "No solution" };
            return grid.Select((cell, i) => new Tuple<int, string>(i, "(" + string.Join(",", cell) + ")"))
                .GroupBy(x => x.Item1 / n)
                .Select(x => x.Select(v => v.Item2))
                .Select(x => string.Join(";", x)).ToArray();
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
