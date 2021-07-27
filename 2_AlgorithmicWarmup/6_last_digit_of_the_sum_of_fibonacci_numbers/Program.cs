using System;

namespace _6_last_digit_of_the_sum_of_fibonacci_numbers
{
    class Program
    {
        private static ulong calcFib(ulong n)
        {
            if (n <= 1)
                return (ulong)n;

            ulong prevNum1 = 0, prevNum2 = 1;

            for (ulong i = 2; i < n; i++)
            {
                var tmpPrev1 = prevNum1;
                prevNum1 = prevNum2;
                prevNum2 += tmpPrev1;
            }

            return prevNum1 + prevNum2;
        }

        private static ulong getLastDigitOfSum(ulong n)
        {
            var fibn2 = calcFib(n + 2) - 1;
            return fibn2 % 10;
        }
        static void Main(string[] args)
        {
            var number = ulong.Parse(Console.ReadLine());
            var digit = getLastDigitOfSum(number);
            Console.WriteLine(digit);
        }
    }
}
