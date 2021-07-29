using System;

namespace _2_last_digit_of_fibonacci_number
{
    public class FibonacciLastDigit
    {
        private static int GetFibonacciLastDigit(uint n)
        {
            if (n <= 1)
                return (int)n;

            int prevDigit = 0, currentDigit = 1;

            for (int i = 2; i <= (int)(n % 60); ++i)
            {
                int tmpPrevDigit = prevDigit;
                prevDigit = currentDigit;
                currentDigit = (tmpPrevDigit + currentDigit) % 10;
            }

            return currentDigit;
        }

        public static string Calc(string line)
        {
            var n = uint.Parse(line);
            return GetFibonacciLastDigit(n).ToString();
        }

        public static void Main(string[] args)
        {
            var line = Console.ReadLine();
            Console.WriteLine(Calc(line));
        }
    }
}
