using System;

namespace _6_last_digit_of_the_sum_of_fibonacci_numbers
{
    public class LastDigitOfSum
    {
        private static int GetFibonacciLastDigit(ulong n)
        {
            int fmod = (int)(n % 60);
            if (fmod <= 1)
                return (int)fmod;

            int prevDigit = 0, currentDigit = 1;

            for (int i = 2; i <= fmod; ++i)
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

        public static string Calc(string input)
        {
            var number = ulong.Parse(input);
            return GetLastDigitOfSum(number).ToString();
        }

        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            Console.WriteLine(Calc(input));
        }
    }
}
