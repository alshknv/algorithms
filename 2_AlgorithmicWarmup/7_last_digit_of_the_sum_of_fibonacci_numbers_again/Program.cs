using System;

namespace _7_last_digit_of_the_sum_of_fibonacci_numbers_again
{
    public class LastDigitOfPartSum
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

        private static int GetLastDigitOfSum(ulong n)
        {
            var fibn2 = GetFibonacciLastDigit(n + 2) - 1;
            return fibn2 >= 0 ? fibn2 : fibn2 + 10;
        }

        private static int GetLastDigitOfPartialSum(ulong m, ulong n)
        {
            var ld = GetLastDigitOfSum(n) - GetLastDigitOfSum(m - 1);
            return ld >= 0 ? ld : 10 + ld;
        }

        public static string Calc(string input)
        {
            var numbers = input.Split(' ');
            return GetLastDigitOfPartialSum(ulong.Parse(numbers[0]), ulong.Parse(numbers[1])).ToString();
        }

        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            Console.WriteLine(Calc(input));
        }
    }
}
