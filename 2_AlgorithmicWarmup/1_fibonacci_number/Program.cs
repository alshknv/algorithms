using System;

namespace _1_fibonacci_number
{
    public class Fibonacci
    {
        private static ulong calcFib(int n)
        {
            if (n <= 1)
                return (ulong)n;

            ulong prevNum1 = 0, prevNum2 = 1;

            for (int i = 2; i < n; i++)
            {
                var tmpPrev1 = prevNum1;
                prevNum1 = prevNum2;
                prevNum2 += tmpPrev1;
            }

            return prevNum1 + prevNum2;
        }

        public static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            Console.WriteLine(calcFib(n));
        }
    }
}
