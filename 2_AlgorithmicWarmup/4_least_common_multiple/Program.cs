using System;

namespace _4_least_common_multiple
{
    public class LCM
    {
        private static uint getGCD(uint a, uint b)
        {
            uint mod;
            do
            {
                mod = a % b;
                a = b;
                b = mod;
            } while (b > 0);
            return a;
        }

        private static ulong getLCM(uint a, uint b)
        {
            return (ulong)a * b / getGCD(a, b);
        }

        public static void Main(string[] args)
        {
            var numbers = Console.ReadLine().Split(' ');
            Console.WriteLine(getLCM(uint.Parse(numbers[0]), uint.Parse(numbers[1])));
        }
    }
}
