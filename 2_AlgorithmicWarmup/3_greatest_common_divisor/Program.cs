using System;

namespace _3_greatest_common_divisor
{
    public class GCD
    {
        private static uint getGCD(uint a, uint b)
        {
            uint mod = 0;
            do
            {
                mod = a % b;
                a = b;
                b = mod;
            } while (b > 0);
            return a;
        }

        public static string Calc(string input)
        {
            var numbers = input.Split(' ');
            return getGCD(uint.Parse(numbers[0]), uint.Parse(numbers[1])).ToString();
        }

        public static void Main(string[] args)
        {
            var input = Console.ReadLine();
            Console.WriteLine(Calc(input));
        }
    }
}
