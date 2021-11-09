using System.Linq;
using System.Collections.Generic;
using System;

namespace _3_ad_allocation
{
    public static class AdAllocation
    {
        private static void Pivot(decimal[][] tableaux, int i0, int j0)
        {
            var p = tableaux[i0][j0];
            for (int i = 1; i < tableaux.Length; i++)
            {
                for (int j = 1; j < tableaux[i].Length; j++)
                {
                    if (i == i0 && j == j0)
                    {
                        tableaux[i][j] = 1 / p;
                    }
                    else if (i == i0)
                    {
                        tableaux[i][j] /= p;
                    }
                    else if (j == j0)
                    {
                        tableaux[i][j] = -(tableaux[i][j] / p);
                    }
                    else
                    {
                        tableaux[i][j] -= (tableaux[i0][j] * tableaux[i][j0] / p);
                    }
                }
            }

            // swap indices
            var buf = tableaux[0][j0];
            tableaux[0][j0] = tableaux[i0][0];
            tableaux[i0][0] = buf;
        }

        public static string[] Solve(string[] input)
        {
            // init simplex tableaux
            var nm = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var tableaux = new decimal[nm[0] + 2][];
            for (int i = 1; i <= tableaux.Length; i++)
            {
                tableaux[i] = new decimal[nm[1] + 2];
                tableaux[i][0] = -i; // indices of y
                var coeff = input[i + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
                for (int j = 1; j <= coeff.Length; j++)
                {
                    tableaux[i][0] = j; // indices of x
                    tableaux[i][j] = coeff[j - 1];
                }
            }
            var b = input[nm[0] + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
            for (int i = 1; i <= b.Length; i++)
            {
                tableaux[i][nm[1] + 1] = b[i];
            }

            var c = input[nm[0] + 2].Split(' ').Select(x => int.Parse(x)).ToArray();

            for (int j = 1; j <= c.Length; j++)
            {
                tableaux[nm[0] + 1][j] = c[j];
            }
            tableaux[nm[0] + 1][nm[1] + 1] = 0;

            // simplex method
            while (true)
            {
                var k = 0;
                for (int i = 1; i <= nm[0]; i++)
                {
                    if (tableaux[i][nm[1] + 1] < 0)
                    {
                        k = i; break;
                    }
                }

                if (k == 0)
                {
                    // case 1 b >= 0
                    var j0 = 0;
                    for (int j = 1; j <= nm[1]; j++)
                    {
                        if (tableaux[nm[0] + 1][j] < 0)
                        {
                            j0 = j; break;
                        }
                    }
                    if (j0 == 0)
                    {
                        // we are done, bounded solution
                        return new string[] { "Bounded solution", "" };
                    }
                    else
                    {
                        var minRatio = decimal.MaxValue;
                        var i0 = 0;
                        for (int i = 1; i <= nm[0]; i++)
                        {
                            if (tableaux[i][j0] > 0)
                            {
                                var ratio = tableaux[i][nm[1] + 1] / tableaux[i][j0];
                                if (ratio < minRatio)
                                {
                                    minRatio = ratio;
                                    i0 = i;
                                }
                            }
                        }
                        if (i0 == 0)
                        {
                            // infinite solution
                            return new string[] { "Infinite" };
                        }
                        else
                        {
                            // pivot around i0j0
                            Pivot(tableaux, i0, j0);
                        }
                    }
                }
                else
                {
                    // case 2 negative b at row k
                    var j0 = 0;
                    for (int j = 1; j <= nm[1]; j++)
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
                        var minRatio = tableaux[k][nm[1] + 1] / tableaux[k][j0];
                        var i0 = k;
                        for (int i = 1; i <= nm[0]; i++)
                        {
                            if (tableaux[i][nm[1] + 1] >= 0 && tableaux[i][j0] > 0)
                            {
                                var ratio = tableaux[i][nm[1] + 1] / tableaux[i][j0];
                                if (ratio < minRatio)
                                {
                                    minRatio = ratio;
                                    i0 = i;
                                }
                            }
                        }
                        // pivot around i0j0
                        Pivot(tableaux, i0, j0);
                    }
                }
            }
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
