using System;

namespace _3_maximum_value_of_an_arithmetic_expression
{
    public static class MaximumValueOfArithmeticExpression
    {
        private static int Min(params int[] values)
        {
            Array.Sort(values);
            return values[0];
        }

        private static int Max(params int[] values)
        {
            Array.Sort(values, (v1, v2) => -v1.CompareTo(v2));
            return values[0];
        }

        private static int Oper(int a, int b, char oper)
        {
            switch (oper)
            {
                case '+': return a + b;
                case '-': return a - b;
                default: return a * b;
            }
        }

        private static int[] MinMax(int[,] min, int[,] max, char[] opers, int i, int j)
        {
            var result = new int[2] { int.MaxValue, int.MinValue };
            for (int k = i; k <= j - 1; k++)
            {
                var a = Oper(max[i, k], max[k + 1, j], opers[k]);
                var b = Oper(max[i, k], min[k + 1, j], opers[k]);
                var c = Oper(min[i, k], max[k + 1, j], opers[k]);
                var d = Oper(min[i, k], min[k + 1, j], opers[k]);
                result[0] = Min(result[0], a, b, c, d);
                result[1] = Max(result[1], a, b, c, d);
            }
            return result;
        }

        private static int MaxParenthesis(int[] digits, char[] opers)
        {
            var n = digits.Length;
            var min = new int[n, n];
            var max = new int[n, n];

            for (int s = 0; s < n; s++)
            {
                for (int i = 0; i < n - s; i++)
                {
                    var j = i + s;
                    if (i == j)
                    {
                        min[i, j] = digits[i];
                        max[i, j] = digits[i];
                    }
                    else
                    {
                        var minmax = MinMax(min, max, opers, i, j);
                        min[i, j] = minmax[0];
                        max[i, j] = minmax[1];
                    }
                }
            }
            return max[0, n - 1];
        }

        public static string Solve(string input)
        {
            var opers = new char[input.Length / 2];
            var digits = new int[input.Length / 2 + 1];
            for (int i = 0; i < input.Length; i++)
            {
                if (i % 2 == 0)
                {
                    digits[i / 2] = int.Parse(input[i].ToString());
                }
                else
                {
                    opers[i / 2] = input[i];
                }
            }
            return MaxParenthesis(digits, opers).ToString();
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Solve(Console.ReadLine()));
        }
    }
}
