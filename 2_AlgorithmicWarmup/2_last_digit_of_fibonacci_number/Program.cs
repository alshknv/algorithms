using System;

namespace _2_last_digit_of_fibonacci_number
{
    public class FibonacciLastDigit
    {
        private static int getFibonacciLastDigit(int n)
        {
            if (n <= 1)
                return n;

            int prevDigit = 0, currentDigit = 1;

            for (int i = 2; i <= n; ++i)
            {
                int tmpPrevDigit = prevDigit;
                prevDigit = currentDigit;
                currentDigit = (tmpPrevDigit + currentDigit) % 10;
            }

            return currentDigit;
        }

        public static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            Console.WriteLine(getFibonacciLastDigit(n));
        }
    }
}
