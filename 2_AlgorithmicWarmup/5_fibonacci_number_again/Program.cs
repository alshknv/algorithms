using System;
using System.Collections.Generic;
using System.Linq;

namespace _5_fibonacci_number_again
{
    public class FibonacciModulo
    {
        private static List<int> GetPisanoPeriod(int m)
        {
            var result = new List<int>() {
                0, 1
            };
            do
            {
                var next = (result[^2] + result[^1]) % m;
                result.Add(next);
            } while (result[^2] != 0 || result[^1] != 1);
            result.RemoveRange(result.Count - 2, 2);
            return result;
        }

        private static long GetFibMod(long n, int m)
        {
            var pisano = GetPisanoPeriod(m);
            return pisano[(int)(n % pisano.Count)];
        }

        public static string Calc(string input)
        {
            var numbers = input.Split(' ');
            return GetFibMod(long.Parse(numbers[0]), int.Parse(numbers[1])).ToString();
        }

        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            Console.WriteLine(Calc(input));
        }
    }
}
