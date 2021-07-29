using System;

namespace _8_last_digit_of_the_sum_of_squares_of_fibonacci_numbers
{
    public class LastDigitOfSumOfSquares
    {
        private static int GetFibonacciLastDigit(ulong n)
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

        public static string Calc(string input)
        {
            var number = ulong.Parse(input);
            return (GetFibonacciLastDigit(number) * GetFibonacciLastDigit(number + 1) % 10).ToString();
        }

        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            Console.WriteLine(Calc(input));
        }
    }
}
