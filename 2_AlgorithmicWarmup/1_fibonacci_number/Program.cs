using System;

namespace fibonacci_number
{
    public class Fibonacci
    {
        private static ulong CalcFib(uint n)
        {
            if (n <= 1)
                return (ulong)n;

            ulong prevNum1 = 0, prevNum2 = 1;

            for (uint i = 2; i < n; i++)
            {
                var tmpPrev1 = prevNum1;
                prevNum1 = prevNum2;
                prevNum2 += tmpPrev1;
            }

            return prevNum1 + prevNum2;
        }

        public static string Calc(string line)
        {
            var n = uint.Parse(line);
            return CalcFib(n).ToString();
        }

        public static void Main(string[] args)
        {
            var line = Console.ReadLine();
            Console.WriteLine(Calc(line));
        }
    }
}
