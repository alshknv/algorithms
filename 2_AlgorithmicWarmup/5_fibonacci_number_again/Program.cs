using System;
using System.Collections.Generic;
using System.Linq;

namespace _5_fibonacci_number_again
{
    class FibonacciHuge
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

        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            var numbers = input.Split(' ');
            var fibMod = GetFibMod(long.Parse(numbers[0]), int.Parse(numbers[1]));
            Console.WriteLine(fibMod);
        }
    }
}
