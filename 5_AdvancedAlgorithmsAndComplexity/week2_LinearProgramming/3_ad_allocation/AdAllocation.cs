using System.Linq;
using System.Collections.Generic;
using System;

namespace _3_ad_allocation
{
    public static class AdAllocation
    {
        private static double[][] Pivot(double[][] tableaux, int i0, int j0)
        {
            var newTableaux = new double[tableaux.Length][];
            for (int i = 0; i < tableaux.Length; i++)
            {
                newTableaux[i] = new double[tableaux[i].Length];
                for (int j = 0; j < tableaux[i].Length; j++)
                {
                    if (j == 0 || i == 0)
                    {
                        if (j == j0)
                        {
                            newTableaux[i][j] = tableaux[i0][0];
                        }
                        else if (i == i0)
                        {
                            newTableaux[i][j] = tableaux[0][j0];
                        }
                        else
                        {
                            newTableaux[i][j] = tableaux[i][j];
                        }
                    }
                    else if (i == i0 && j == j0)
                    {
                        newTableaux[i][j] = 1 / tableaux[i0][j0];
                    }
                    else if (i == i0)
                    {
                        newTableaux[i][j] = tableaux[i][j] / tableaux[i0][j0];
                    }
                    else if (j == j0)
                    {
                        newTableaux[i][j] = -(tableaux[i][j] / tableaux[i0][j0]);
                    }
                    else
                    {
                        newTableaux[i][j] = tableaux[i][j] - (tableaux[i0][j] * tableaux[i][j0] / tableaux[i0][j0]);
                    }
                }
            }
            return newTableaux;
        }

        private static string[] BoundedSolution(double[][] tableaux)
        {
            var n = tableaux.Length - 2;
            var m = tableaux[0].Length - 2;
            const double zero = 0;
            var solution = new string[m];

            for (int i = 1; i <= n; i++)
            {
                if (tableaux[i][0] <= 0) continue;
                solution[(int)tableaux[i][0] - 1] = tableaux[i][tableaux[i].Length - 1].ToString("0.000000000000000000").Replace(",", ".");
            }
            for (int j = 1; j <= m; j++)
            {
                if (tableaux[0][j] <= 0) continue;
                solution[(int)tableaux[0][j] - 1] = zero.ToString("0.000000000000000000").Replace(",", ".");
            }
            return new string[] { "Bounded solution", string.Join(" ", solution) };
        }

        private static string[] Simplex(double[][] tableaux)
        {
            var rnd = new Random();
            var n = tableaux.Length - 2;
            var m = tableaux[0].Length - 2;
            while (true)
            {
                var k = 0;
                for (int i = 1; i <= n; i++)
                {
                    if (Math.Round(tableaux[i][m + 1], 5) < 0)
                    {
                        k = i; break;
                    }
                }

                if (k == 0)
                {
                    // case 1 b >= 0
                    var j0 = 0;
                    for (int j = 1; j <= m; j++)
                    {
                        // find negative -c in last row
                        if (Math.Round(tableaux[n + 1][j], 5) < 0)
                        {
                            j0 = j; break;
                        }
                    }
                    if (j0 == 0)
                    {
                        // if all -c are positive, we are done, it's bounded solution
                        return BoundedSolution(tableaux);
                    }
                    else
                    {
                        // choose row(s) with minimum b/a ratio
                        var minRatio = double.MaxValue;
                        var i0list = new List<int>();
                        for (int i = 1; i <= n; i++)
                        {
                            if (Math.Round(tableaux[i][j0], 5) > 0)
                            {
                                var ratio = Math.Round(tableaux[i][m + 1] / tableaux[i][j0], 5);
                                if (ratio < minRatio)
                                {
                                    minRatio = ratio;
                                    i0list = new List<int>() { i };
                                }
                                else if (ratio == minRatio)
                                {
                                    i0list.Add(i);
                                }
                            }
                        }
                        if (i0list.Count == 0)
                        {
                            // no pivots, it's unbounded solution
                            return new string[] { "Infinity" };
                        }
                        else
                        {
                            // choose random pivot from candidates
                            var i0 = i0list[rnd.Next(1000) % i0list.Count];
                            tableaux = Pivot(tableaux, i0, j0);
                        }
                    }
                }
                else
                {
                    // case 2 negative b at row k
                    var j0 = 0;
                    // find negative element in row k
                    for (int j = m; j >= 1; j--)
                    {
                        if (Math.Round(tableaux[k][j], 5) < 0)
                        {
                            j0 = j;
                            break;
                        }
                    }
                    if (j0 == 0)
                    {
                        // all elements in row k are positive, but b is negative, there's no solution
                        return new string[] { "No solution" };
                    }
                    else
                    {
                        // find pivot candidates with minimum b/a ratio
                        var minRatio = Math.Round(tableaux[k][m + 1] / tableaux[k][j0], 5);
                        var i0list = new List<int>() { k };
                        for (int i = 1; i <= n; i++)
                        {
                            if (Math.Round(tableaux[i][m + 1], 5) >= 0 && Math.Round(tableaux[i][j0], 5) > 0)
                            {
                                var ratio = Math.Round(tableaux[i][m + 1] / tableaux[i][j0], 5);
                                if (ratio < minRatio)
                                {
                                    minRatio = ratio;
                                    i0list = new List<int>() { i };
                                }
                                else if (ratio == minRatio)
                                {
                                    i0list.Add(i);
                                }
                            }
                        }
                        // choose random pivot from candidates
                        var i0 = i0list[rnd.Next(1000) % i0list.Count];
                        tableaux = Pivot(tableaux, i0, j0);
                    }
                }
            }
        }

        public static string[] Solve(string[] input)
        {
            // init simplex tableaux
            var nm = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var tableaux = new double[nm[0] + 2][];
            tableaux[0] = new double[nm[1] + 2];
            for (int i = 1; i <= nm[0]; i++)
            {
                tableaux[i] = new double[nm[1] + 2];
                tableaux[i][0] = -i; // indices of y
                var coeff = input[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                for (int j = 1; j <= coeff.Length; j++)
                {
                    tableaux[0][j] = j; // indices of x
                    tableaux[i][j] = coeff[j - 1];
                }
            }

            // last column b
            var b = input[nm[0] + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
            for (int i = 1; i <= b.Length; i++)
            {
                tableaux[i][nm[1] + 1] = b[i - 1];
            }

            // last row -c
            var c = input[nm[0] + 2].Split(' ').Select(x => int.Parse(x)).ToArray();
            tableaux[nm[0] + 1] = new double[nm[1] + 2];
            for (int j = 1; j <= c.Length; j++)
            {
                tableaux[nm[0] + 1][j] = -c[j - 1];
            }
            tableaux[nm[0] + 1][nm[1] + 1] = 0;

            // simplex method
            return Simplex(tableaux);
        }

        static void Main(string[] args)
        {
            var nmline = Console.ReadLine();
            var n = int.Parse(nmline.Split(' ')[0]);
            var input = new string[n + 3];
            input[0] = nmline;
            for (int i = 1; i <= n; i++)
                input[i] = Console.ReadLine();
            input[n + 1] = Console.ReadLine();
            input[n + 2] = Console.ReadLine();

            foreach (var line in Solve(input))
                Console.WriteLine(line);
        }
    }
}
