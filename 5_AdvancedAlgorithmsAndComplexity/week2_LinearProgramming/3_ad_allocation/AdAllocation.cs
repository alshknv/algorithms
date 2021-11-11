using System.Linq;
using System.Collections.Generic;
using System;

namespace _3_ad_allocation
{
    public static class AdAllocation
    {
        private static decimal[][] Pivot(decimal[][] tableaux, int i0, int j0)
        {
            var newTableaux = new decimal[tableaux.Length][];
            for (int i = 0; i < tableaux.Length; i++)
            {
                newTableaux[i] = new decimal[tableaux[i].Length];
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

        private static string[] BoundedSolution(decimal[][] tableaux)
        {
            var n = tableaux.Length - 2;
            var m = tableaux[0].Length - 2;
            const decimal zero = 0;
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

        private static string[] Simplex(decimal[][] tableaux)
        {
            var n = tableaux.Length - 2;
            var m = tableaux[0].Length - 2;
            while (true)
            {
                var k = 0;
                for (int i = 1; i <= n; i++)
                {
                    if (tableaux[i][m + 1] < 0)
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
                        if (tableaux[n + 1][j] < 0)
                        {
                            j0 = j; break;
                        }
                    }
                    if (j0 == 0)
                    {
                        // we are done, bounded solution
                        return BoundedSolution(tableaux);
                    }
                    else
                    {
                        var minRatio = decimal.MaxValue;
                        var i0 = 0;
                        for (int i = 1; i <= n; i++)
                        {
                            if (tableaux[i][j0] > 0)
                            {
                                var ratio = tableaux[i][m + 1] / tableaux[i][j0];
                                if (ratio < minRatio)
                                {
                                    minRatio = ratio;
                                    i0 = i;
                                }
                            }
                        }
                        if (i0 == 0)
                        {
                            // infinity solution
                            return new string[] { "Infinity" };
                        }
                        else
                        {
                            // pivot around i0j0
                            tableaux = Pivot(tableaux, i0, j0);
                        }
                    }
                }
                else
                {
                    // case 2 negative b at row k
                    var j0 = 0;
                    for (int j = 1; j <= m; j++)
                    {
                        if (tableaux[k][j] < 0)
                        {
                            j0 = j;
                            break;
                        }
                    }
                    if (j0 == 0)
                    {
                        // no solution
                        return new string[] { "No solution" };
                    }
                    else
                    {
                        var minRatio = tableaux[k][m + 1] / tableaux[k][j0];
                        var i0 = k;
                        for (int i = 1; i <= n; i++)
                        {
                            if (tableaux[i][m + 1] >= 0 && tableaux[i][j0] > 0)
                            {
                                var ratio = tableaux[i][m + 1] / tableaux[i][j0];
                                if (ratio < minRatio)
                                {
                                    minRatio = ratio;
                                    i0 = i;
                                }
                            }
                        }
                        // pivot around i0j0
                        tableaux = Pivot(tableaux, i0, j0);
                    }
                }
            }
        }

        public static string[] Solve(string[] input)
        {
            // init simplex tableaux
            var nm = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var tableaux = new decimal[nm[0] + 2][];
            tableaux[0] = new decimal[nm[1] + 2];
            for (int i = 1; i <= nm[0]; i++)
            {
                tableaux[i] = new decimal[nm[1] + 2];
                tableaux[i][0] = -i; // indices of y
                var coeff = input[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                for (int j = 1; j <= coeff.Length; j++)
                {
                    tableaux[0][j] = j; // indices of x
                    tableaux[i][j] = coeff[j - 1];
                }
            }
            var b = input[nm[0] + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
            for (int i = 1; i <= b.Length; i++)
            {
                tableaux[i][nm[1] + 1] = b[i - 1];
            }

            var c = input[nm[0] + 2].Split(' ').Select(x => int.Parse(x)).ToArray();

            tableaux[nm[0] + 1] = new decimal[nm[1] + 2];
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
